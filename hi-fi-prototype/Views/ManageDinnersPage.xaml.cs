using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageDinnersPage : ContentPage
    {
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
                    var copy = new MealViewModel();

                    if (meal.Reservation != null)
                    {
                        copy.Reservation = new ReservationViewModel()
                        {
                            ID = meal.Reservation.ID,
                            OrganiserID = meal.Reservation.OrganiserID,
                            Organiser = meal.Reservation.Organiser,
                            RestaurantID = meal.Reservation.RestaurantID,
                            Restaurant = meal.Reservation.Restaurant,
                            Year = meal.Reservation.Year,
                            Month = meal.Reservation.Month,
                            ExactDateTime = meal.Reservation.ExactDateTime,
                            NegotiatedBeerPrice = meal.Reservation.NegotiatedBeerPrice,
                            NegotiatedBeerDiscount = meal.Reservation.NegotiatedBeerDiscount,
                            IsAmnesty = meal.Reservation.IsAmnesty,
                        };
                    };

                    if (meal.Dinner != null)
                    {
                        copy.Dinner = new DinnerViewModel()
                        {
                            ID = meal.Dinner.ID,
                            ReservationID = meal.Dinner.ReservationID,
                            Reservation = meal.Dinner.Reservation,
                            CostPerPerson = meal.Dinner.CostPerPerson,
                            NumBeersConsumed = meal.Dinner.NumBeersConsumed,
                        };
                    };

                    _meals.Add(copy);
                }
                OnPropertyChanged(nameof(Meals));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
        }

        private async void DinnerItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is MealViewModel mealViewModel)
            {
                if (mealViewModel.Reservation != null)
                {
                    Dictionary<string, object> parameters = new() { { "Reservation", mealViewModel.Reservation } };
                    await Shell.Current.GoToAsync("edit_reservation", true, parameters);
                }
                else if (mealViewModel.Dinner != null)
                {
                    Dictionary<string, object> parameters = new() { { "Dinner", mealViewModel.Dinner } };
                    await Shell.Current.GoToAsync("edit_dinner", true, parameters);
                }
            }
        }

        private async void AddNewReservation_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_reservation", true);
        }

        private async Task LoadData()
        {
            try
            {
                LoadingIndicator.IsRunning = true;
                AddNewDinner.IsEnabled = false;
                DinnerItems.IsEnabled = false;

                await Task.Delay(750);

                var reservationPretendRdbms = new ObservableCollection<ReservationViewModel>()
                {
                    new()
                    {
                        ID = 100,
                        OrganiserID = 4,
                        Organiser = new() { ID = 4, Name = "Reginald Weber" },
                        RestaurantID = 33,
                        Restaurant = new() { ID = 33, Name = "Curry of India", StreetAddress = "71B Lake Road", Suburb = "Devonport", PhoneNumber = "094461077", },
                        Year = 2016,
                        Month = 1,
                        ExactDateTime = DateTime.Now,
                        NegotiatedBeerPrice = null,
                        NegotiatedBeerDiscount = 2.0,
                        IsAmnesty = true,
                    },
                };

                var dinnerPretendRdbms = new ObservableCollection<DinnerViewModel>() {
                    new()
                    {
                        ID = 1,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 1,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 1,
                            OrganiserID = 3,
                            Organiser = new() { ID = 3, Name = "Kaiser Farley" },
                            RestaurantID = 62,
                            Restaurant = new() { ID = 62, Name = "Moti Mahal Indian Restaurant", StreetAddress = "100 Wellington Street", Suburb = "Freemans Bay", PhoneNumber = null, },
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 2,
                        CostPerPerson = 40.55,
                        NumBeersConsumed = 58,
                        ReservationID = 2,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 2,
                            OrganiserID = 1,
                            Organiser = new() { ID = 1, Name = "Alfred Sanchez" },
                            RestaurantID = 26,
                            Restaurant = new() { ID = 26, Name = "Saffron - Fine Indian Cuisine", StreetAddress = "31 Ponsonby Road", Suburb = "Grey Lynn", PhoneNumber = "093782122", },
                            Year = 2015,
                            Month = 10,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = null,
                            NegotiatedBeerDiscount = 2.0,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 3,
                        CostPerPerson = 48.11,
                        NumBeersConsumed = 58,
                        ReservationID = 3,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 3,
                            OrganiserID = 4,
                            Organiser = new() { ID = 4, Name = "Reginald Weber" },
                            RestaurantID = 23,
                            Restaurant = new() { ID = 23, Name = "Nite Spice Indian Restaurant", StreetAddress = "407 Mount Eden Road", Suburb = "Mount Eden", PhoneNumber = null, },
                            Year = 2015,
                            Month = 9,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 4,
                        CostPerPerson = 41.35,
                        NumBeersConsumed = 58,
                        ReservationID = 4,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 4,
                            OrganiserID = 2,
                            Organiser = new() { ID = 2, Name = "Kyle Scott" },
                            RestaurantID = 17,
                            Restaurant = new() { ID = 17, Name = "Indian Flame NZ", StreetAddress = null, Suburb = "Ponsonby", PhoneNumber = null, },
                            Year = 2015,
                            Month = 8,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 7.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 5,
                        CostPerPerson = 44.75,
                        NumBeersConsumed = 58,
                        ReservationID = 5,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 5,
                            OrganiserID = 1,
                            Organiser = new() { ID = 1, Name = "Alfred Sanchez" },
                            RestaurantID = 9,
                            Restaurant = new() { ID = 9, Name = "Aruns Indian Restaurant", StreetAddress = "227 Old Albany Village", Suburb = "Albany", PhoneNumber = "094150991", },
                            Year = 2015,
                            Month = 7,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 6,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 6,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 6,
                            OrganiserID = 5,
                            Organiser = new() { ID = 5, Name = "Cullen Schmitt" },
                            RestaurantID = 13,
                            Restaurant = new() { ID = 13, Name = "Oh Calcutta", StreetAddress = "151 Parnell Road", Suburb = "Parnell", PhoneNumber = "093779090", },
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 7,
                        CostPerPerson = 40.55,
                        NumBeersConsumed = 58,
                        ReservationID = 7,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 7,
                            OrganiserID = 1,
                            Organiser = new() { ID = 1, Name = "Alfred Sanchez" },
                            RestaurantID = 52,
                            Restaurant = new() { ID = 52, Name = "Monsoon Indian Cuisine", StreetAddress = "8 Anzac Road", Suburb = "Browns Bay", PhoneNumber = "094796039", },
                            Year = 2015,
                            Month = 10,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = null,
                            NegotiatedBeerDiscount = 2.0,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 8,
                        CostPerPerson = 48.11,
                        NumBeersConsumed = 58,
                        ReservationID = 8,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 8,
                            OrganiserID = 6,
                            Organiser = new() { ID = 6, Name = "Kenneth Allen" },
                            RestaurantID = 33,
                            Restaurant = new() { ID = 33, Name = "Curry of India", StreetAddress = "71B Lake Road", Suburb = "Devonport", PhoneNumber = "094461077", },
                            Year = 2015,
                            Month = 9,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 9,
                        CostPerPerson = 41.35,
                        NumBeersConsumed = 58,
                        ReservationID = 9,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 9,
                            OrganiserID = 1,
                            Organiser = new() { ID = 1, Name = "Alfred Sanchez" },
                            RestaurantID = 21,
                            Restaurant = new() { ID = 21, Name = "Palki Indian Restaurant", StreetAddress = "279 Parnell Road", Suburb = "Parnell", PhoneNumber = "093573848", },
                            Year = 2015,
                            Month = 8,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 7.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 10,
                        CostPerPerson = 44.75,
                        NumBeersConsumed = 58,
                        ReservationID = 10,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 10,
                            OrganiserID = 7,
                            Organiser = new() { ID = 7, Name = "Jaxtyn Erickson" },
                            RestaurantID = 56,
                            Restaurant = new() { ID = 56, Name = "The Empress", StreetAddress = "2B Surrey Crescent", Suburb = "Grey Lynn", PhoneNumber = "093788780", },
                            Year = 2015,
                            Month = 7,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 11,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 11,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 11,
                            OrganiserID = 6,
                            Organiser = new() { ID = 6, Name = "Kenneth Allen" },
                            RestaurantID = 40,
                            Restaurant = new() { ID = 40, Name = "Anokha Indian Restaurant", StreetAddress = "140 Kitchener Road", Suburb = "Milford", PhoneNumber = "094891499", },
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 12,
                        CostPerPerson = 40.55,
                        NumBeersConsumed = 58,
                        ReservationID = 12,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 12,
                            OrganiserID = 8,
                            Organiser = new() { ID = 8, Name = "Barrett Herrera" },
                            RestaurantID = 42,
                            Restaurant = new() { ID = 42, Name = "Bolliwood indian Restaurant", StreetAddress = "17 Huron Street", Suburb = "Takapuna", PhoneNumber = null, },
                            Year = 2015,
                            Month = 10,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = null,
                            NegotiatedBeerDiscount = 2.0,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 13,
                        CostPerPerson = 48.11,
                        NumBeersConsumed = 58,
                        ReservationID = 13,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 13,
                            OrganiserID = 9,
                            Organiser = new() { ID = 9, Name = "Lee Dickerson" },
                            RestaurantID = 48,
                            Restaurant = new() { ID = 48, Name = "Heritage Cuisine of India", StreetAddress = "501 New North Road", Suburb = "Kingsland", PhoneNumber = "098151700", },
                            Year = 2015,
                            Month = 9,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 14,
                        CostPerPerson = 41.35,
                        NumBeersConsumed = 58,
                        ReservationID = 14,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 14,
                            OrganiserID = 2,
                            Organiser = new() { ID = 2, Name = "Kyle Scott" },
                            RestaurantID = 45,
                            Restaurant = new() { ID = 45, Name = "Clove Indian Cuisine", StreetAddress = "17 Huron Street", Suburb = "Takapuna", PhoneNumber = "094899911", },
                            Year = 2015,
                            Month = 8,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 7.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 15,
                        CostPerPerson = 44.75,
                        NumBeersConsumed = 58,
                        ReservationID = 15,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 15,
                            OrganiserID = 10,
                            Organiser = new() { ID = 10, Name = "Edwin Spears" },
                            RestaurantID = 37,
                            Restaurant = new() { ID = 37, Name = "Mr India Restaurant Northcote", StreetAddress = "31 Northcote Road", Suburb = "Hillcrest", PhoneNumber = "094185162", },
                            Year = 2015,
                            Month = 7,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 16,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 16,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 16,
                            OrganiserID = 11,
                            Organiser = new() { ID = 11, Name = "Luke Wood" },
                            RestaurantID = 32,
                            Restaurant = new() { ID = 32, Name = "Everest Dine", StreetAddress = "193 Parnell Road", Suburb = "Parnell", PhoneNumber = "093032468", },
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 17,
                        CostPerPerson = 40.55,
                        NumBeersConsumed = 58,
                        ReservationID = 17,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 17,
                            OrganiserID = 7,
                            Organiser = new() { ID = 7, Name = "Jaxtyn Erickson" },
                            RestaurantID = 30,
                            Restaurant = new() { ID = 30, Name = "Kashmir Milford Indian Restaurant", StreetAddress = "140 Kitchener Road", Suburb = "Milford", PhoneNumber = null, },
                            Year = 2015,
                            Month = 10,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = null,
                            NegotiatedBeerDiscount = 2.0,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 18,
                        CostPerPerson = 48.11,
                        NumBeersConsumed = 58,
                        ReservationID = 18,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 18,
                            OrganiserID = 3,
                            Organiser = new() { ID = 3, Name = "Kaiser Farley" },
                            RestaurantID = 29,
                            Restaurant = new() { ID = 29, Name = "Yellow Chili Indian Restaurant", StreetAddress = "204 Jervois Road", Suburb = "Herne Bay", PhoneNumber = "093762001", },
                            Year = 2015,
                            Month = 9,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 19,
                        CostPerPerson = 41.35,
                        NumBeersConsumed = 58,
                        ReservationID = 19,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 19,
                            OrganiserID = 4,
                            Organiser = new() { ID = 4, Name = "Reginald Weber" },
                            RestaurantID = 27,
                            Restaurant = new() { ID = 27, Name = "iVillage At Victoria", StreetAddress = "210-218 Victoria Street West", Suburb = "Auckland CBD", PhoneNumber = "093094009", },
                            Year = 2015,
                            Month = 8,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 7.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 20,
                        CostPerPerson = 44.75,
                        NumBeersConsumed = 58,
                        ReservationID = 20,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 20,
                            OrganiserID = 6,
                            Organiser = new() { ID = 6, Name = "Kenneth Allen" },
                            RestaurantID = 31,
                            Restaurant = new() { ID = 31, Name = "Handi Tandoori", StreetAddress = "60 Federal Street", Suburb = "Auckland", PhoneNumber = null, },
                            Year = 2015,
                            Month = 7,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 21,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 21,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 21,
                            OrganiserID = 8,
                            Organiser = new() { ID = 8, Name = "Barrett Herrera" },
                            RestaurantID = 25,
                            Restaurant = new() { ID = 25, Name = "Indique", StreetAddress = "18 Birkenhead Avenue", Suburb = "Birkenhead", PhoneNumber = "094807211", },
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 22,
                        CostPerPerson = 40.55,
                        NumBeersConsumed = 58,
                        ReservationID = 22,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 22,
                            OrganiserID = 9,
                            Organiser = new() { ID = 9, Name = "Lee Dickerson" },
                            RestaurantID = 34,
                            Restaurant = new() { ID = 34, Name = "Pushkar Indian Cuisine", StreetAddress = "178 Hurstmere Road", Suburb = "Takapuna", PhoneNumber = "094862950", },
                            Year = 2015,
                            Month = 10,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = null,
                            NegotiatedBeerDiscount = 2.0,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 23,
                        CostPerPerson = 48.11,
                        NumBeersConsumed = 58,
                        ReservationID = 23,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 23,
                            OrganiserID = 8,
                            Organiser = new() { ID = 8, Name = "Barrett Herrera" },
                            RestaurantID = 35,
                            Restaurant = new() { ID = 35, Name = "Dabbawala", StreetAddress = "Beresford Square", Suburb = "Auckland Central", PhoneNumber = null, },
                            Year = 2015,
                            Month = 9,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 24,
                        CostPerPerson = 41.35,
                        NumBeersConsumed = 58,
                        ReservationID = 24,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 24,
                            OrganiserID = 7,
                            Organiser = new() { ID = 7, Name = "Jaxtyn Erickson" },
                            RestaurantID = 36,
                            Restaurant = new() { ID = 36, Name = "Big Tikka", StreetAddress = "458 Lake Road", Suburb = "Takapuna", PhoneNumber = "02041779130", },
                            Year = 2015,
                            Month = 8,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 7.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 25,
                        CostPerPerson = 44.75,
                        NumBeersConsumed = 58,
                        ReservationID = 25,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 25,
                            OrganiserID = 6,
                            Organiser = new() { ID = 6, Name = "Kenneth Allen" },
                            RestaurantID = 38,
                            Restaurant = new() { ID = 38, Name = "Raviz Indian Cuisine", StreetAddress = "61 Hobson Street", Suburb = "Auckland CBD", PhoneNumber = "093098800", },
                            Year = 2015,
                            Month = 7,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 26,
                        CostPerPerson = 45.25,
                        NumBeersConsumed = 58,
                        ReservationID = 26,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 26,
                            OrganiserID = 5,
                            Organiser = new() { ID = 5, Name = "Cullen Schmitt" },
                            RestaurantID = 45,
                            Restaurant = new() { ID = 45, Name = "Clove Indian Cuisine", StreetAddress = "17 Huron Street", Suburb = "Takapuna", PhoneNumber = "094899911", },
                            Year = 2015,
                            Month = 11,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 27,
                        CostPerPerson = 40.55,
                        NumBeersConsumed = 58,
                        ReservationID = 27,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 27,
                            OrganiserID = 4,
                            Organiser = new() { ID = 4, Name = "Reginald Weber" },
                            RestaurantID = 39,
                            Restaurant = new() { ID = 39, Name = "The Curry Master", StreetAddress = "18 Birkenhead Avenue", Suburb = "Birkenhead", PhoneNumber = "094807211", },
                            Year = 2015,
                            Month = 10,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = null,
                            NegotiatedBeerDiscount = 2.0,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 28,
                        CostPerPerson = 48.11,
                        NumBeersConsumed = 58,
                        ReservationID = 28,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 28,
                            OrganiserID = 4,
                            Organiser = new() { ID = 4, Name = "Reginald Weber" },
                            RestaurantID = 41,
                            Restaurant = new() { ID = 41, Name = "Empress of India", StreetAddress = "2B Surrey Crescent", Suburb = "Grey Lynn", PhoneNumber = null, },
                            Year = 2015,
                            Month = 9,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 5.5,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 29,
                        CostPerPerson = 41.35,
                        NumBeersConsumed = 58,
                        ReservationID = 29,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 29,
                            OrganiserID = 3,
                            Organiser = new() { ID = 3, Name = "Kaiser Farley" },
                            RestaurantID = 24,
                            Restaurant = new() { ID = 24, Name = "Nite Spice Indian Restaurant", StreetAddress = "471 Khyber Pass Road", Suburb = "Newmarket", PhoneNumber = "095291897", },
                            Year = 2015,
                            Month = 8,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 7.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 30,
                        CostPerPerson = 44.75,
                        NumBeersConsumed = 58,
                        ReservationID = 30,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 30,
                            OrganiserID = 2,
                            Organiser = new() { ID = 2, Name = "Kyle Scott" },
                            RestaurantID = 43,
                            Restaurant = new() { ID = 43, Name = "Shahi Herne bay", StreetAddress = "26 Jervois Road", Suburb = "Ponsonby", PhoneNumber = "093788896", },
                            Year = 2015,
                            Month = 7,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 31,
                        CostPerPerson = 41.35,
                        NumBeersConsumed = 58,
                        ReservationID = 29,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 29,
                            OrganiserID = 3,
                            Organiser = new() { ID = 3, Name = "Kaiser Farley" },
                            RestaurantID = 24,
                            Restaurant = new() { ID = 24, Name = "Nite Spice Indian Restaurant", StreetAddress = "471 Khyber Pass Road", Suburb = "Newmarket", PhoneNumber = "095291897", },
                            Year = 2015,
                            Month = 8,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 7.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                    new()
                    {
                        ID = 32,
                        CostPerPerson = 44.75,
                        NumBeersConsumed = 58,
                        ReservationID = 30,
                        Reservation = new ReservationViewModel()
                        {
                            ID = 30,
                            OrganiserID = 4,
                            Organiser = new() { ID = 4, Name = "Reginald Weber" },
                            RestaurantID = 44,
                            Restaurant = new() { ID = 44, Name = "Flying Rickshaw Indian Eatery", StreetAddress = "97 Victoria Road", Suburb = "Devonport", PhoneNumber = "094457721", },
                            Year = 2015,
                            Month = 7,
                            ExactDateTime = DateTime.Now,
                            NegotiatedBeerPrice = 6.0,
                            NegotiatedBeerDiscount = null,
                            IsAmnesty = false,
                        },
                    },
                };

                _totalPages = (reservationPretendRdbms.Count + dinnerPretendRdbms.Count) / PageSize;
                if ((reservationPretendRdbms.Count + dinnerPretendRdbms.Count) % PageSize > 0)
                    _totalPages++;
                _currentPage = Math.Min(Math.Max(0, _currentPage), _totalPages - 1);

                LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;

                if (_currentPage == 0)
                    MergePageIntoListView(reservationPretendRdbms.Take(1).ToList());
                MergePageIntoListView(dinnerPretendRdbms.Skip(_currentPage * PageSize).Take(PageSize).ToList());

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

        private void MergePageIntoListView(List<ReservationViewModel> pageOfReservations)
        {
            DinnerItems.BatchBegin();
            foreach (var reservation in pageOfReservations)
            {
                var x = _meals.FirstOrDefault(y => y.Reservation != null && reservation.ID == y.Reservation.ID);
                if (x == null)
                {
                    _meals.Add(new MealViewModel()
                    {
                        Reservation = new ReservationViewModel()
                        {
                            ID = reservation.ID,
                            OrganiserID = reservation.OrganiserID,
                            Organiser = reservation.Organiser,
                            RestaurantID = reservation.RestaurantID,
                            Restaurant = reservation.Restaurant,
                            Year = reservation.Year,
                            Month = reservation.Month,
                            ExactDateTime = reservation.ExactDateTime,
                            NegotiatedBeerPrice = reservation.NegotiatedBeerPrice,
                            NegotiatedBeerDiscount = reservation.NegotiatedBeerDiscount,
                            IsAmnesty = reservation.IsAmnesty,
                        },
                        Dinner = null,
                    });
                }
            }
            DinnerItems.BatchCommit();
        }

        private void MergePageIntoListView(List<DinnerViewModel> pageOfDinners)
        {
            DinnerItems.BatchBegin();
            foreach (var dinner in pageOfDinners)
            {
                var x = _meals.FirstOrDefault(y => y.Dinner != null && dinner.ID == y.Dinner.ID);
                if (x == null)
                {
                    _meals.Add(new MealViewModel()
                    {
                        Reservation = null,
                        Dinner = new DinnerViewModel()
                        {
                            ID = dinner.ID,
                            ReservationID = dinner.ReservationID,
                            Reservation = dinner.Reservation,
                            CostPerPerson = dinner.CostPerPerson,
                            NumBeersConsumed = dinner.NumBeersConsumed,
                        },
                    });
                }
            }
            DinnerItems.BatchCommit();
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
