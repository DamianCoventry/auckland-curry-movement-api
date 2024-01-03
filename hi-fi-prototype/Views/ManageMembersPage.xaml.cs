using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageMembersPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<MemberViewModel> _members = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageMembersPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int PageSize { get; set; } = 10;

        public ObservableCollection<MemberViewModel> Members
        {
            get => _members;
            set
            {
                _members = [];
                foreach (var member in value)
                {
                    var copy = new MemberViewModel()
                    {
                        ID = member.ID,
                        Name = member.Name,
                        SponsorID = member.SponsorID,
                        Sponsor = member.Sponsor,
                        CurrentLevelID = member.CurrentLevelID,
                        CurrentLevel = member.CurrentLevel,
                        AttendanceCount = member.AttendanceCount,
                        IsArchived = member.IsArchived,
                        ArchiveReason = member.ArchiveReason,
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
            if (e.Item is MemberViewModel memberViewModel)
            {
                Dictionary<string, object> parameters = new() { { "Member", memberViewModel } };
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
                var pageOfData = await AcmService.ListClubMembersAsync(1, _currentPage * PageSize, PageSize);
                if (pageOfData != null && pageOfData.PageItems != null)
                {
                    _totalPages = pageOfData.TotalPages;
                    LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
                    MergePageIntoListView(pageOfData.PageItems);
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

        private void MergePageIntoListView(List<acm_models.Member> pageOfMembers)
        {
            MemberItems.BatchCommit();
            foreach (var member in pageOfMembers)
            {
                var existing = _members.FirstOrDefault(y => y.ID == member.ID);
                if (existing == null)
                {
                    var vm = MemberViewModel.FromModel(member);
                    if (vm != null)
                        _members.Add(vm);
                }
                else
                {
                    existing.ID = member.ID;
                    existing.Name = member.Name;
                    existing.SponsorID = member.SponsorID;
                    existing.Sponsor = MemberViewModel.FromModel(member.Sponsor); // TODO: check for recursion
                    existing.CurrentLevelID = member.CurrentLevelID;
                    existing.CurrentLevel = LevelViewModel.FromModel(member.CurrentLevel);
                    existing.AttendanceCount = member.AttendanceCount;
                    existing.IsArchived = member.IsArchived;
                    existing.ArchiveReason = member.ArchiveReason;
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
