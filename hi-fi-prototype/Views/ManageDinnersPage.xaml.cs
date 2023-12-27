using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageDinnersPage : ContentPage
    {
        private ObservableCollection<DinnerViewModel> _dinners = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageDinnersPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int PageSize { get; set; } = 10;

        public ObservableCollection<DinnerViewModel> Dinners
        {
            get => _dinners;
            set
            {
                _dinners = [];
                foreach (var dinner in value)
                {
                    var copy = new DinnerViewModel
                    {
                        ID = dinner.ID,
                        ReservationID = dinner.ReservationID,
                        Reservation = dinner.Reservation,
                        CostPerPerson = dinner.CostPerPerson,
                        NumBeersConsumed = dinner.NumBeersConsumed,
                    };
                    _dinners.Add(copy);
                }
                OnPropertyChanged(nameof(Dinners));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
        }

        private async void DinnerItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is DinnerViewModel dinnerViewModel)
            {
                Dictionary<string, object> parameters = new() { { "Dinner", dinnerViewModel } };
                await Shell.Current.GoToAsync("edit_dinner", true, parameters);
            }
        }

        private async void AddNewDinner_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_dinner", true);
        }

        private async Task LoadData()
        {
            try
            {
                LoadingIndicator.IsRunning = true;
                AddNewDinner.IsEnabled = false;
                DinnerItems.IsEnabled = false;

                await Task.Delay(750);

                var pretendRdbms = new ObservableCollection<DinnerViewModel>() {
                    new()
                    {
                        ID = 1,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 1,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 1,
                            OrganiserID = 1,
                            Organiser = null, // TODO: MemberViewModel.FromModel(...),
                            RestaurantID = 1,
                            Restaurant = null, // TODO: RestaurantViewModel.FromModel(...),
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 1,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 1,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 1,
                            OrganiserID = 1,
                            Organiser = null, // TODO: MemberViewModel.FromModel(...),
                            RestaurantID = 1,
                            Restaurant = null, // TODO: RestaurantViewModel.FromModel(...),
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 1,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 1,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 1,
                            OrganiserID = 1,
                            Organiser = null, // TODO: MemberViewModel.FromModel(...),
                            RestaurantID = 1,
                            Restaurant = null, // TODO: RestaurantViewModel.FromModel(...),
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 1,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 1,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 1,
                            OrganiserID = 1,
                            Organiser = null, // TODO: MemberViewModel.FromModel(...),
                            RestaurantID = 1,
                            Restaurant = null, // TODO: RestaurantViewModel.FromModel(...),
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 1,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 1,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 1,
                            OrganiserID = 1,
                            Organiser = null, // TODO: MemberViewModel.FromModel(...),
                            RestaurantID = 1,
                            Restaurant = null, // TODO: RestaurantViewModel.FromModel(...),
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
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
                AddNewDinner.IsEnabled = true;
                DinnerItems.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<DinnerViewModel> pageOfDinners)
        {
            bool changed = false;
            foreach (var dinner in pageOfDinners)
            {
                var x = _dinners.FirstOrDefault(y => y.ID == dinner.ID);
                if (x == null)
                {
                    _dinners.Add(new DinnerViewModel()
                    {
                        ID = dinner.ID,
                        ReservationID = dinner.ReservationID,
                        Reservation = dinner.Reservation,
                        CostPerPerson = dinner.CostPerPerson,
                        NumBeersConsumed = dinner.NumBeersConsumed,
                    });
                    changed = true;
                }
            }
            if (changed)
                OnPropertyChanged(nameof(Dinners));
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
