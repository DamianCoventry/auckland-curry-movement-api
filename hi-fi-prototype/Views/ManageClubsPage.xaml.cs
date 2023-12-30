using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageClubsPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<ClubViewModel> _clubs = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageClubsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int PageSize { get; set; } = 10;

        public ObservableCollection<ClubViewModel> Clubs
        {
            get => _clubs;
            set
            {
                _clubs = [];
                foreach (var club in value)
                {
                    _clubs.Add(new ClubViewModel
                    {
                        ID = club.ID,
                        Name = club.Name,
                        NumMembers = club.NumMembers,
                        ArchiveReason = club.ArchiveReason,
                        IsArchived = club.IsArchived,
                        FoundingFathers = [],
                    });
                }
                OnPropertyChanged(nameof(Clubs));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfClubs(); });
        }

        private async void ClubItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is ClubViewModel clubViewModel)
            {
                Dictionary<string, object> parameters = new() { { "Club", clubViewModel } };
                await Shell.Current.GoToAsync("edit_club", true, parameters);
            }
        }

        private async void AddNewClub_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_club", true);
        }

        private void RefreshClubList_Clicked(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfClubs(); });
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task DownloadSinglePageOfClubs()
        {
            try
            {
                if (LoadingIndicator.IsRunning)
                    return;

                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                RefreshClubList.IsEnabled = false;
                AddNewClub.IsEnabled = false;
                ClubItems.IsEnabled = false;

                var clubsPageOfData = await AcmService.ListClubsAsync(_currentPage * PageSize, PageSize);
                if (clubsPageOfData != null && clubsPageOfData.PageItems != null)
                {
                    _totalPages = clubsPageOfData.TotalPages;
                    LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
                    MergePageIntoListView(clubsPageOfData.PageItems);
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
                RefreshClubList.IsEnabled = true;
                AddNewClub.IsEnabled = true;
                ClubItems.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<acm_models.Club> pageOfClubs)
        {
            ClubItems.BatchBegin();
            foreach (var club in pageOfClubs)
            {
                var exists = _clubs.FirstOrDefault(y => y.ID == club.ID);
                if (exists == null)
                {
                    var viewModel = ClubViewModel.FromModel(club);
                    if (viewModel != null)
                        _clubs.Add(viewModel);
                }
            }
            ClubItems.BatchCommit();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfClubs(); });
            }
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
