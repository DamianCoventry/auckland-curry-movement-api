using acm_mobile_app.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace acm_mobile_app
{
    public partial class MainPage : ContentPage
    {
        private const int PAGE_SIZE = 10;
        private int _page = 0;
        private int _numDinnersReturnedLastTime = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(RefreshDinnerData);
        }

        public void RefreshData(object sender, EventArgs e)
        {
            Task.Run(RefreshDinnerData);
        }

        public void OnAddObject(object sender, EventArgs e)
        {
            ObjectGrid.IsVisible = !ObjectGrid.IsVisible;
        }

        public async void OnAddReservation(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("edit_reservation");
        }

        public async void OnAddRestaurant(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("edit_restaurant");
        }

        public async void OnAddDinner(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("edit_member");
        }

        public async void NavigateToEditLevel(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("edit_level");
        }

        public async void NavigateToEditClub(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("edit_club");
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        public int CurrentPage { get { return _page + 1; } }

        public class CollatedDinnerInfo
        {
            public string Organiser { get; set; } = string.Empty;
            public string Restaurant { get; set; } = string.Empty;
            public DateTime ExactDateTime { get; set; }
            public float NegotiatedBeerPrice { get; set; }
            public float NegotiatedBeerDiscount { get; set; }
            public float CostPerPerson { get; set; }
            public int NumBeersConsumed { get; set; }
        }

        public ObservableCollection<CollatedDinnerInfo> CollatedDinnerInfos { get; set; } = [];

        private async Task RefreshDinnerData()
        {
            try
            {
                var dinners = await AcmService.ListDinnersAsync(_page * PAGE_SIZE, PAGE_SIZE);
                _numDinnersReturnedLastTime = dinners.Count;

                CollatedDinnerInfos.Clear();
                foreach (var dinner in dinners)
                {
                    var reservation = await AcmService.GetReservationAsync(dinner.ReservationID);
                    var member = await AcmService.GetMemberAsync(reservation.OrganiserID);
                    var restaurant = await AcmService.GetRestaurantAsync(reservation.RestaurantID);

                    CollatedDinnerInfos.Add(new CollatedDinnerInfo()
                    {
                        Organiser = member.Name,
                        Restaurant = restaurant.Name,
                        ExactDateTime = reservation.ExactDateTime,
                        NegotiatedBeerPrice = (float)(reservation.NegotiatedBeerPrice == null ? 0.0 : reservation.NegotiatedBeerPrice),
                        NegotiatedBeerDiscount = (float)(reservation.NegotiatedBeerDiscount == null ? 0.0 : reservation.NegotiatedBeerDiscount),
                        CostPerPerson = (float)(dinner.CostPerPerson == null ? 0.0 : dinner.CostPerPerson),
                        NumBeersConsumed = (int)(dinner.NumBeersConsumed == null ? 0 : dinner.NumBeersConsumed)
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Close");
            }
            finally
            {
                ClubDinners.EndRefresh();
            }
        }

        public void OnPreviousPage(object o, EventArgs e)
        {
            if (_page > 0)
            {
                --_page;
                Task.Run(RefreshDinnerData);
                CurrentPageNumber.Text = (1 + _page).ToString();
            }
        }

        public void OnNextPage(object o, EventArgs e)
        {
            if (_numDinnersReturnedLastTime >= PAGE_SIZE)
            {
                ++_page;
                Task.Run(RefreshDinnerData);
                CurrentPageNumber.Text = (1 + _page).ToString();
            }
        }
    }
}
