using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace hi_fi_prototype.Views
{
    public partial class ManageRestaurantsPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<RestaurantViewModel> _restaurants = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageRestaurantsPage()
        {
            InitializeComponent();
            BindingContext = this;

            DeleteRestaurantCommand = new Command<int>(
                execute: async (int id) =>
                {
                    bool yes = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this restaurant?", "Yes", "No");
                    if (yes)
                        await SendDeleteRestaurantMessage(id);
                },
                canExecute: (int id) =>
                {
                    var x = Restaurants.FirstOrDefault(x => x.ID == id);
                    return x != null && !x.IsDeleting;
                });
        }

        public int PageSize { get; set; } = 10;

        public ICommand DeleteRestaurantCommand { private set; get; }

        public ObservableCollection<RestaurantViewModel> Restaurants
        {
            get => _restaurants;
            set
            {
                _restaurants = [];
                foreach (var restaurant in value)
                {
                    if (restaurant.ID > 1)
                    {
                        var copy = new RestaurantViewModel
                        {
                            ID = restaurant.ID,
                            Name = restaurant.Name,
                            StreetAddress = restaurant.StreetAddress,
                            Suburb = restaurant.Suburb,
                            PhoneNumber = restaurant.PhoneNumber,
                            ArchiveReason = restaurant.ArchiveReason,
                            IsArchived = restaurant.IsArchived,
                        };
                        _restaurants.Add(copy);
                    }
                }
                OnPropertyChanged(nameof(Restaurants));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfRestaurants(); });
        }

        private void RefreshRestaurantsList_Clicked(object sender, EventArgs e)
        {
            Restaurants = [];
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfRestaurants(); });
        }

        private async void RestaurantItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is RestaurantViewModel restaurantViewModel)
            {
                Dictionary<string, object> parameters = new() { { "Restaurant", restaurantViewModel } };
                await Shell.Current.GoToAsync("edit_restaurant", true, parameters);
            }
        }

        private async void AddNewRestaurant_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_restaurant", true);
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task DownloadSinglePageOfRestaurants()
        {
            try
            {
                if (LoadingIndicator.IsRunning)
                    return;

                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                AddNewRestaurant.IsEnabled = false;
                RestaurantItems.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;
                RefreshRestaurantsList.IsEnabled = false;

                var pageOfData = await AcmService.ListRestaurantsAsync(_currentPage * PageSize, PageSize);
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
                AddNewRestaurant.IsEnabled = true;
                RestaurantItems.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
                RefreshRestaurantsList.IsEnabled = true;
            }
        }

        private async Task SendDeleteRestaurantMessage(int id)
        {
            bool deleteSucceeded = false;
            try
            {
                var startTime = DateTime.Now;

                var deleting = Restaurants.FirstOrDefault(x => x.ID == id);
                if (deleting != null)
                    deleting.IsDeleting = true; // Shows an activity indicator

                AddNewRestaurant.IsEnabled = false;
                RestaurantItems.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;
                RefreshRestaurantsList.IsEnabled = false;

                deleteSucceeded = await AcmService.DeleteRestaurantAsync(id);
                if (deleteSucceeded)
                {
                    var elapsed = DateTime.Now - startTime;
                    if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS * 2)
                        await Task.Delay(MIN_REFRESH_TIME_MS * 2 - elapsed.Milliseconds);

                    if (deleting != null)
                        Restaurants.Remove(deleting);
                }
                else
                    await DisplayAlert("Delete Failed", $"Unable to delete the restaurant with the {id}.", "Close");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                AddNewRestaurant.IsEnabled = true;
                RestaurantItems.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
                RefreshRestaurantsList.IsEnabled = true;
            }

            if (deleteSucceeded)
                await DownloadSinglePageOfRestaurants();
        }

        private void MergePageIntoListView(List<acm_models.Restaurant> pageOfRestaurants)
        {
            RestaurantItems.BatchBegin();
            foreach (var restaurant in pageOfRestaurants)
            {
                if (restaurant.ID > 1)
                {
                    var existing = _restaurants.FirstOrDefault(y => y.ID == restaurant.ID);
                    if (existing == null)
                    {
                        var vm = RestaurantViewModel.FromModel(restaurant);
                        if (vm != null)
                            _restaurants.Add(vm);
                    }
                    else
                    {
                        existing.ID = restaurant.ID;
                        existing.Name = restaurant.Name;
                        existing.StreetAddress = restaurant.StreetAddress;
                        existing.Suburb = restaurant.Suburb;
                        existing.PhoneNumber = existing.Suburb;
                        existing.IsArchived = restaurant.IsArchived;
                        existing.ArchiveReason = restaurant.ArchiveReason;
                    }
                }
            }
            RestaurantItems.BatchCommit();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfRestaurants(); });
            }
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
