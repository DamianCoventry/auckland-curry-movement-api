using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

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

            DeleteClubCommand = new Command<int>(
                execute: async (int id) =>
                {
                    bool yes = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this club?", "Yes", "No");
                    if (yes)
                        await SendDeleteClubMessage(id);
                },
                canExecute: (int id) =>
                {
                    var x = Clubs.FirstOrDefault(x => x.ID == id);
                    return x != null && !x.IsDeleting;
                });
        }

        public int PageSize { get; set; } = 10;

        public ICommand DeleteClubCommand { private set; get; }

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
            Clubs = [];
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
                LoadMoreButton.IsEnabled = false;
                AddNewClub.IsEnabled = false;
                ClubItems.IsEnabled = false;

                var pageOfData = await AcmService.ListClubsAsync(_currentPage * PageSize, PageSize);
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
                RefreshClubList.IsEnabled = true;
                AddNewClub.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
                ClubItems.IsEnabled = true;
                //(MainScrollView as IView).InvalidateMeasure();
            }
        }

        private async Task SendDeleteClubMessage(int id)
        {
            bool deleteSucceeded = false;
            try
            {
                var startTime = DateTime.Now;

                var deletingClub = Clubs.FirstOrDefault(x => x.ID == id);
                if (deletingClub != null)
                    deletingClub.IsDeleting = true; // Shows an activity indicator

                RefreshClubList.IsEnabled = false;
                AddNewClub.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;
                ClubItems.IsEnabled = false;

                deleteSucceeded = await AcmService.DeleteClubAsync(id);
                if (deleteSucceeded)
                {
                    var elapsed = DateTime.Now - startTime;
                    if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS*2)
                        await Task.Delay(MIN_REFRESH_TIME_MS*2 - elapsed.Milliseconds);

                    if (deletingClub != null)
                        Clubs.Remove(deletingClub);
                }
                else
                    await DisplayAlert("Delete Failed", $"Unable to delete the club with the {id}.", "Close");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                RefreshClubList.IsEnabled = true;
                AddNewClub.IsEnabled = true;
                ClubItems.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
            }

            if (deleteSucceeded)
                await DownloadSinglePageOfClubs();
        }

        private void MergePageIntoListView(List<acm_models.Club> pageOfClubs)
        {
            ClubItems.BatchBegin();
            foreach (var club in pageOfClubs)
            {
                var existing = _clubs.FirstOrDefault(y => y.ID == club.ID);
                if (existing == null)
                {
                    var viewModel = ClubViewModel.FromModel(club);
                    if (viewModel != null)
                        _clubs.Add(viewModel);
                }
                else
                {
                    existing.ID = club.ID;
                    existing.Name = club.Name;
                    existing.NumMembers = club.Memberships != null ? club.Memberships.Count : 0;
                    existing.IsDeleting = false;
                    existing.IsArchived = club.IsArchived;
                    existing.ArchiveReason = club.ArchiveReason;
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
