using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class SelectMultipleMembersPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<SelectedMemberViewModel> _selectedMembers = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public SelectMultipleMembersPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Action<List<MembershipViewModel>>? AcceptFunction { get; set; } = null;
        public Action? CancelFunction { get; set; } = null;
        public List<MembershipViewModel>? SelectedMembers { get; set; } = null;
        private List<MembershipViewModel>? OriginalSelection { get; set; } = null;
        public int PageSize { get; set; } = 10;

        public ObservableCollection<SelectedMemberViewModel> Members
        {
            get => _selectedMembers;
            set
            {
                _selectedMembers = [];
                foreach (var member in value)
                {
                    _selectedMembers.Add(new SelectedMemberViewModel()
                    {
                        IsSelected = member.IsSelected,
                        ID = member.ID,
                        Name = member.Name,
                        ArchiveReason = member.ArchiveReason,
                        IsArchived = member.IsArchived,
                    });
                }
                OnPropertyChanged(nameof(Members));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (SelectedMembers != null)
            {
                OriginalSelection = [];
                foreach (var  selectedMember in SelectedMembers)
                {
                    var m = selectedMember.Member;
                    if (m == null) continue;

                    OriginalSelection.Add(new MembershipViewModel()
                    {
                        Member = new MemberViewModel()
                        {
                            ID = m.ID, Name = m.Name, ArchiveReason = m.ArchiveReason, IsArchived = m.IsArchived,
                        }
                    });
                }
            }

            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfMembers(); });
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task DownloadSinglePageOfMembers()
        {
            try
            {
                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                AcceptChanges.IsEnabled = false;
                DiscardChanges.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;

                // TODO: Get club id from somewhere
                var memberships = await AcmService.ListClubMembershipsAsync(1, _currentPage * PageSize, PageSize);
                if (memberships != null && memberships.PageItems != null)
                {
                    _totalPages = memberships.TotalPages;
                    MergePageOfMembers(memberships.PageItems);
                    LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
                }

                var elapsed = DateTime.Now - startTime;
                if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                    await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                AcceptChanges.IsEnabled = true;
                DiscardChanges.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
            }
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
            CancelFunction?.Invoke();
            await Navigation.PopAsync();
        }

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            if (!IsMemberSelected)
            {
                var toast = Toast.Make("Please select at fewest 1 member");
                await toast.Show();
                return;
            }

            SelectedMembers = [];
            foreach (var selectedMember in _selectedMembers)
            {
                if (selectedMember.IsSelected)
                {
                    SelectedMembers.Add(new MembershipViewModel()
                    {
                        Member = new MemberViewModel()
                        {
                            ID = selectedMember.ID,
                            Name = selectedMember.Name,
                            IsArchived = selectedMember.IsArchived,
                            ArchiveReason = selectedMember.ArchiveReason,
                        },
                    });
                }
            }
            AcceptFunction?.Invoke(SelectedMembers);
            await Navigation.PopAsync();
        }

        private void MergePageOfMembers(List<acm_models.Membership> memberships)
        {
            MemberItems.BatchBegin();
            foreach (var ms in memberships)
            {
                var exists = _selectedMembers.FirstOrDefault(y => y.ID == ms.MemberID);
                if (exists == null)
                {
                    var m = ms.Member;
                    if (m == null) continue;

                    _selectedMembers.Add(new SelectedMemberViewModel()
                    {
                        ID = m.ID,
                        Name = m.Name,
                        ArchiveReason = m.ArchiveReason,
                        IsArchived = m.IsArchived,
                        IsSelected = IsOriginallySelected(ms),
                    });
                }
            }
            MemberItems.BatchCommit();
        }

        private bool IsOriginallySelected(acm_models.Membership ms)
        {
            if (OriginalSelection == null || OriginalSelection.Count == 0)
                return false;
            var m = ms.Member;
            if (m == null)
                return false;

            var originallySelected = OriginalSelection.FirstOrDefault(os => os.Member != null && os.Member.ID == m.ID && os.Member.Name == m.Name);
            if (originallySelected != null)
            {
                OriginalSelection.Remove(originallySelected);
                return true;
            }

            return false;
        }

        private bool IsMemberSelected
        {
            get
            {
                foreach (var member in _selectedMembers)
                    if (member.IsSelected) return true;
                return false;
            }
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfMembers(); });
            }
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
