using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageRestaurantsPage : ContentPage
    {
        private ObservableCollection<RestaurantViewModel> _restaurants = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageRestaurantsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int PageSize { get; set; } = 10;

        public ObservableCollection<RestaurantViewModel> Restaurants
        {
            get => _restaurants;
            set
            {
                _restaurants = [];
                foreach (var restaurant in value)
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
                OnPropertyChanged(nameof(Restaurants));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
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

        private async Task LoadData()
        {
            try
            {
                LoadingIndicator.IsRunning = true;
                AddNewRestaurant.IsEnabled = false;
                RestaurantItems.IsEnabled = false;

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
            }
        }

        private void MergePageIntoListView(List<RestaurantViewModel> pageOfRestaurants)
        {
            bool changed = false;
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
                        ArchiveReason = restaurant.ArchiveReason,
                        IsArchived = restaurant.IsArchived,
                    });
                    changed = true;
                }
            }
            if (changed)
                OnPropertyChanged(nameof(Restaurants));
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
