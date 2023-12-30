using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class SelectSingleRestaurantPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<RestaurantViewModel> _restaurants = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public SelectSingleRestaurantPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Action<RestaurantViewModel>? AcceptFunction { get; set; } = null;
        public Action? CancelFunction { get; set; } = null;
        public RestaurantViewModel? SelectedRestaurant { get; set; } = null;
        private RestaurantViewModel? OriginalSelection { get; set; } = null;
        public int PageSize { get; set; } = 10;

        public ObservableCollection<RestaurantViewModel> Restaurants
        {
            get => _restaurants;
            set
            {
                _restaurants = [];
                foreach (var restaurant in value)
                {
                    _restaurants.Add(new RestaurantViewModel()
                    {
                        ID = restaurant.ID,
                        Name = restaurant.Name,
                        StreetAddress = restaurant.StreetAddress,
                        Suburb = restaurant.Suburb,
                        PhoneNumber = restaurant.PhoneNumber,
                        IsArchived = restaurant.IsArchived,
                        ArchiveReason = restaurant.ArchiveReason,
                    });
                }
                OnPropertyChanged(nameof(Restaurants));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (SelectedRestaurant != null)
            {
                OriginalSelection = new RestaurantViewModel()
                {
                    ID = SelectedRestaurant.ID,
                    Name = SelectedRestaurant.Name,
                    StreetAddress = SelectedRestaurant.StreetAddress,
                    Suburb = SelectedRestaurant.Suburb,
                    PhoneNumber = SelectedRestaurant.PhoneNumber,
                    IsArchived = SelectedRestaurant.IsArchived,
                    ArchiveReason = SelectedRestaurant.ArchiveReason,
                };
            }

            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
        }

        private async Task LoadData()
        {
            try
            {
                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                AcceptChanges.IsEnabled = false;
                DiscardChanges.IsEnabled = false;

                await Task.Delay(750);
                var pretendRdbms = new ObservableCollection<RestaurantViewModel>() {
                    new()
                    {
                        ID = 1,
                        Name = "Sages Indian Restaurant",
                        StreetAddress = "407 Mount Eden Road",
                        Suburb = "Mount Eden",
                        PhoneNumber = "095291897",
                    },
                    new()
                    {
                        ID = 2,
                        Name = "India Gate - Indian Restaurant",
                        StreetAddress = "60 Federal Street",
                        Suburb = "Devonport",
                        PhoneNumber = "02041779130",
                    },
                    new()
                    {
                        ID = 3,
                        Name = "Aruns Indian Restaurant",
                        StreetAddress = "458 Lake Road",
                        Suburb = "Takapuna",
                    },
                    new()
                    {
                        ID = 4,
                        Name = "iVillage At Victoria",
                        StreetAddress = "140 Kitchener Road",
                        Suburb = "Browns Bay",
                        PhoneNumber = "094796039",
                    },
                    new()
                    {
                        ID = 6,
                        Name = "Curry of India",
                        StreetAddress = "501 New North Road",
                        Suburb = "Grey Lynn",
                        PhoneNumber = "094451546",
                    },
                };

                _totalPages = pretendRdbms.Count / PageSize;
                if (pretendRdbms.Count % PageSize > 0)
                    _totalPages++;
                _currentPage = Math.Min(Math.Max(0, _currentPage), _totalPages - 1);

                LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;

                MergePageIntoListView(pretendRdbms.Skip(_currentPage * PageSize).Take(PageSize).ToList());

                await Task.Delay(750);

                if (OriginalSelection != null)
                {
                    foreach (var restaurant in Restaurants)
                    {
                        if (OriginalSelection != null && OriginalSelection.ID == restaurant.ID && OriginalSelection.Name == restaurant.Name)
                        {
                            OriginalSelection = null;
                            RestaurantItems.SelectedItem = restaurant;
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
            }
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
            CancelFunction?.Invoke();
            await Navigation.PopAsync();
        }

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            if (RestaurantItems.SelectedItem == null)
            {
                var toast = Toast.Make("Please select a restaurant");
                await toast.Show();
                return;
            }

            if (RestaurantItems.SelectedItem is RestaurantViewModel x)
            {
                SelectedRestaurant = x;
                AcceptFunction?.Invoke(SelectedRestaurant);
                await Navigation.PopAsync();
            }
        }

        private void MergePageIntoListView(List<RestaurantViewModel> pageOfRestaurants)
        {
            RestaurantItems.BatchBegin();
            foreach (var restaurant in pageOfRestaurants)
            {
                var x = _restaurants.FirstOrDefault(y => y.ID == restaurant.ID);
                if (x == null)
                {
                    _restaurants.Add(new RestaurantViewModel()
                    {
                        ID = restaurant.ID,
                        Name = restaurant.Name,
                        StreetAddress = restaurant.StreetAddress,
                        Suburb = restaurant.Suburb,
                        PhoneNumber = restaurant.PhoneNumber,
                        IsArchived = restaurant.IsArchived,
                        ArchiveReason = restaurant.ArchiveReason,
                    });
                }
            }
            RestaurantItems.BatchCommit();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
            }
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
