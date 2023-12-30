using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class SelectMultipleMembersPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<SelectedMemberViewModel> _members = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public SelectMultipleMembersPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Action<List<MemberViewModel>>? AcceptFunction { get; set; } = null;
        public Action? CancelFunction { get; set; } = null;
        public List<MemberViewModel>? SelectedMembers { get; set; } = null;
        private List<MemberViewModel>? OriginalSelection { get; set; } = null;
        public int PageSize { get; set; } = 10;

        public ObservableCollection<SelectedMemberViewModel> Members
        {
            get => _members;
            set
            {
                _members = [];
                foreach (var member in value)
                {
                    _members.Add(new SelectedMemberViewModel()
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
                    OriginalSelection.Add(new MemberViewModel()
                    {
                        ID = selectedMember.ID, Name = selectedMember.Name,
                        ArchiveReason = selectedMember.ArchiveReason,
                        IsArchived = selectedMember.IsArchived,
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

                // TODO: Get club id from somewhere
                var pageOfData = await AcmService.ListClubMembersAsync(1, _currentPage * PageSize, PageSize);
                if (pageOfData != null && pageOfData.PageItems != null)
                {
                    _totalPages = pageOfData.TotalPages;
                    MergePageOfMembers(pageOfData.PageItems);
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
            foreach (var member in _members)
            {
                if (member.IsSelected)
                {
                    SelectedMembers.Add(new MemberViewModel()
                    {
                        ID = member.ID,
                        Name = member.Name,
                        IsArchived = member.IsArchived,
                        ArchiveReason = member.ArchiveReason,
                    });
                }
            }
            AcceptFunction?.Invoke(SelectedMembers);
            await Navigation.PopAsync();
        }

        private void MergePageOfMembers(List<acm_models.Member> pageOfMembers)
        {
            MemberItems.BatchBegin();
            foreach (var member in pageOfMembers)
            {
                var exists = _members.FirstOrDefault(y => y.ID == member.ID);
                if (exists == null)
                {
                    _members.Add(new SelectedMemberViewModel()
                    {
                        ID = member.ID,
                        Name = member.Name,
                        ArchiveReason = member.ArchiveReason,
                        IsArchived = member.IsArchived,
                        IsSelected = IsOriginallySelected(member),
                    });
                }
            }
            MemberItems.BatchCommit();
        }

        private bool IsOriginallySelected(acm_models.Member member)
        {
            if (OriginalSelection == null || OriginalSelection.Count == 0)
                return false;

            var originallySelected = OriginalSelection.FirstOrDefault(x => x.ID == member.ID && x.Name == member.Name);
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
                foreach (var member in _members)
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
