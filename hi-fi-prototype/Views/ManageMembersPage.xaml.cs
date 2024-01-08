using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageMembersPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<MembershipViewModel> _members = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageMembersPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

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
                    var copy = new MembershipViewModel()
                    {
                        SponsorID = ms.SponsorID,
                        Sponsor = ms.Sponsor,
                        LevelID = ms.LevelID,
                        Level = ms.Level,
                        AttendanceCount = ms.AttendanceCount,
                        IsAdmin = ms.IsAdmin,
                        IsFoundingFather = ms.IsFoundingFather,
                        IsAuditor = ms.IsAuditor,
                        Member = new MemberViewModel()
                        {
                            ID = m.ID,
                            Name = m.Name,
                            IsArchived = m.IsArchived,
                            ArchiveReason = m.ArchiveReason,
                        },
                    };
                    _members.Add(copy);
                }
                OnPropertyChanged(nameof(Members));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfMembers(); });
        }

        private void RefreshMembersList_Clicked(object sender, EventArgs e)
        {
            Members = [];
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfMembers(); });
        }

        private async void MemberItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is MembershipViewModel memberViewModel)
            {
                Dictionary<string, object> parameters = new() { { "Membership", memberViewModel } };
                await Shell.Current.GoToAsync("edit_member", true, parameters);
            }
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task DownloadSinglePageOfMembers()
        {
            try
            {
                if (LoadingIndicator.IsRunning)
                    return;

                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                MemberItems.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;

                // TODO: Get the club id from somewhere
                var memberships = await AcmService.ListClubMembershipsAsync(1, _currentPage * PageSize, PageSize);
                if (memberships != null && memberships.PageItems != null)
                {
                    _totalPages = memberships.TotalPages;
                    LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
                    MergePageIntoListView(memberships.PageItems);
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
                MemberItems.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<acm_models.Membership> memberships)
        {
            MemberItems.BatchCommit();
            foreach (var ms in memberships)
            {
                var existing = _members.FirstOrDefault(x => x.MemberID == ms.MemberID);
                if (existing == null)
                {
                    var vm = MembershipViewModel.FromModel(ms);
                    if (vm != null)
                        _members.Add(vm);
                }
                else
                {
                    existing.SponsorID = ms.SponsorID;
                    existing.Sponsor = MembershipViewModel.FromModel(ms.Sponsor); // TODO: check for recursion
                    existing.LevelID = ms.LevelID;
                    existing.Level = LevelViewModel.FromModel(ms.Level);
                    existing.AttendanceCount = ms.AttendanceCount;
                    existing.IsAdmin = ms.IsAdmin;
                    existing.IsFoundingFather = ms.IsFoundingFather;
                    existing.IsAuditor = ms.IsAuditor;
                    existing.IsArchived = ms.IsArchived;
                    existing.ArchiveReason = ms.ArchiveReason;
                    if (ms.Member != null)
                        existing.Member = new MemberViewModel() { ID = ms.Member.ID, Name = ms.Member.Name };
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
