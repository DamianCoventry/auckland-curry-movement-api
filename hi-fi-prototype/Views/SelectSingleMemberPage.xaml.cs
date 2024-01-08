using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class SelectSingleMemberPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<MembershipViewModel> _members = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public SelectSingleMemberPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Action<MembershipViewModel>? AcceptFunction { get; set; } = null;
        public Action? CancelFunction { get; set; } = null;
        public MembershipViewModel? SelectedMember { get; set; } = null;
        private MembershipViewModel? OriginalSelection { get; set; } = null;
        public int PageSize { get; set; } = 10;

        public ObservableCollection<MembershipViewModel> Members
        {
            get => _members;
            set
            {
                _members = [];
                foreach (var ms in value)
                {
                    var m = ms.Member;
                    if (m == null) continue;

                    _members.Add(new MembershipViewModel()
                    {
                        Member = new MemberViewModel() { ID = m.ID, Name = m.Name, ArchiveReason = m.ArchiveReason, IsArchived = m.IsArchived, },
                    });
                }
                OnPropertyChanged(nameof(Members));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (SelectedMember != null && SelectedMember.Member != null)
            {
                var m = SelectedMember.Member;
                OriginalSelection = new MembershipViewModel()
                {
                    Member = new MemberViewModel() { ID = m.ID, Name = m.Name, ArchiveReason = m.ArchiveReason, IsArchived = m.IsArchived, },
                };
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

                // TODO: Get the club ID from somewhere
                var memberships = await AcmService.ListClubMembershipsAsync(1, _currentPage * PageSize, PageSize);
                if (memberships != null && memberships.PageItems != null)
                {
                    _totalPages = memberships.TotalPages;
                    LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
                    MergePageIntoListView(memberships.PageItems);
                }

                if (OriginalSelection != null)
                {
                    foreach (var ms in Members)
                    {
                        var m = OriginalSelection?.Member;
                        if (m == null) continue;

                        if (m.ID == ms.MemberID && m.Name == ms.Member?.Name)
                        {
                            OriginalSelection = null;
                            MemberItems.SelectedItem = ms;
                        }
                    }
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
            if (MemberItems.SelectedItem == null)
            {
                var toast = Toast.Make("Please select a member");
                await toast.Show();
                return;
            }

            if (MemberItems.SelectedItem is MembershipViewModel x)
            {
                SelectedMember = x;
                AcceptFunction?.Invoke(SelectedMember);
                await Navigation.PopAsync();
            }
        }

        private void MergePageIntoListView(List<acm_models.Membership> memberships)
        {
            MemberItems.BatchBegin();
            foreach (var ms in memberships)
            {
                var exists = _members.FirstOrDefault(x => x.MemberID == ms.MemberID);
                if (exists == null)
                {
                    var vm = MembershipViewModel.FromModel(ms);
                    if (vm != null)
                        _members.Add(vm);
                }
            }
            MemberItems.BatchCommit();
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
