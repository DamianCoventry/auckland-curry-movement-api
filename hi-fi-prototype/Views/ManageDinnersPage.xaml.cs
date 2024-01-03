using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageDinnersPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<MealViewModel> _meals = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageDinnersPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int PageSize { get; set; } = 10;

        public ObservableCollection<MealViewModel> Meals
        {
            get => _meals;
            set
            {
                _meals = [];
                foreach (var meal in value)
                {
                    _meals.Add(new MealViewModel()
                    {
                        ReservationID = meal.ReservationID,
                        OrganiserID = meal.OrganiserID,
                        OrganiserName = meal.OrganiserName,
                        RestaurantID = meal.RestaurantID,
                        RestaurantName = meal.RestaurantName,
                        Year = meal.Year,
                        Month = meal.Month,
                        ExactDateTime = meal.ExactDateTime,
                        NegotiatedBeerPrice = meal.NegotiatedBeerPrice,
                        NegotiatedBeerDiscount = meal.NegotiatedBeerDiscount,
                        IsAmnesty = meal.IsAmnesty,
                        DinnerID = meal.DinnerID,
                        CostPerPerson = meal.CostPerPerson,
                        NumBeersConsumed = meal.NumBeersConsumed,
                    });
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfDinners(); });
        }

        private void RefreshDinnersList_Clicked(object sender, EventArgs e)
        {
            Meals = [];
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfDinners(); });
        }

        private async void DinnerItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is MealViewModel vm)
            {
                var reservation = new ReservationViewModel()
                {
                    ID = vm.ReservationID,
                    OrganiserID = vm.OrganiserID,
                    Organiser = new MemberViewModel() { ID = vm.OrganiserID, Name = vm.OrganiserName },
                    RestaurantID = vm.RestaurantID,
                    Restaurant = new RestaurantViewModel() { ID = vm.RestaurantID, Name = vm.RestaurantName },
                    Year = vm.Year,
                    Month = vm.Month,
                    ExactDateTime = vm.ExactDateTime,
                    NegotiatedBeerPrice = vm.NegotiatedBeerPrice,
                    NegotiatedBeerDiscount = vm.NegotiatedBeerDiscount,
                    IsAmnesty = vm.IsAmnesty,
                };

                if (vm.DinnerID == null)
                {
                    Dictionary<string, object> parameters = new() { { "Reservation", reservation } };
                    await Shell.Current.GoToAsync("edit_reservation", true, parameters);
                }
                else
                {
                    Dictionary<string, object> parameters = new() {
                        { "Dinner", new DinnerViewModel()
                            {
                                ID = vm.DinnerID, CostPerPerson = vm.CostPerPerson, NumBeersConsumed = vm.NumBeersConsumed,
                                ReservationID = vm.ReservationID, Reservation = reservation,
                            }
                        }
                    };
                    await Shell.Current.GoToAsync("edit_dinner", true, parameters);
                }
            }
        }

        private async void AddNewReservation_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_reservation", true);
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task DownloadSinglePageOfDinners()
        {
            try
            {
                if (LoadingIndicator.IsRunning)
                    return;

                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                AddNewDinner.IsEnabled = false;
                RefreshDinnersList.IsEnabled = false;
                DinnerItems.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;

                // TODO: Get the club id from somewhere
                var pageOfData = await AcmService.ListClubMealsAsync(1, _currentPage * PageSize, PageSize);
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
                AddNewDinner.IsEnabled = true;
                RefreshDinnersList.IsEnabled = true;
                DinnerItems.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<acm_models.Meal> mealItems)
        {
            DinnerItems.BatchBegin();
            foreach (var meal in mealItems)
            {
                var existing = _meals.FirstOrDefault(y => y.ReservationID == meal.ReservationID);
                if (existing == null)
                {
                    var mealViewModel = MealViewModel.FromModel(meal);
                    if (mealViewModel != null)
                        _meals.Add(mealViewModel);
                }
                else
                {
                    existing.ReservationID = meal.ReservationID;
                    existing.OrganiserID = meal.OrganiserID;
                    existing.OrganiserName = meal.OrganiserName;
                    existing.RestaurantID = meal.RestaurantID;
                    existing.RestaurantName = meal.RestaurantName;
                    existing.Year = meal.Year;
                    existing.Month = meal.Month;
                    existing.ExactDateTime = meal.ExactDateTime;
                    existing.NegotiatedBeerPrice = meal.NegotiatedBeerPrice;
                    existing.NegotiatedBeerDiscount = meal.NegotiatedBeerDiscount;
                    existing.IsAmnesty = meal.IsAmnesty;
                    existing.DinnerID = meal.DinnerID;
                    existing.CostPerPerson = meal.CostPerPerson;
                    existing.NumBeersConsumed = meal.NumBeersConsumed;
                }
            }
            DinnerItems.BatchCommit();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfDinners(); });
            }
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
