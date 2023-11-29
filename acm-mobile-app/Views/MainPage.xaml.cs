using Microsoft.Identity.Client;
using System.Diagnostics;

namespace acm_mobile_app
{
    public partial class MainPage : ContentPage
    {
        public List<Models.Restaurant> Restaurants { get; set; } = [
                new Models.Restaurant() { ID = 1, Name = "Little India", StreetAddress = "142 West Coast Road" },
                new Models.Restaurant() { ID = 2, Name = "Moo's Backyard", StreetAddress = "37 Albany Road" },
                new Models.Restaurant() { ID = 3, Name = "Satya South Indian", StreetAddress = "515 Sandringham Road" },
                new Models.Restaurant() { ID = 4, Name = "Kabana Indian Cuisine", StreetAddress = "598 New North Road" },
            ];

        public List<Models.Reservation> Reservations { get; set; } = [
                new Models.Reservation() { ID = 1, OrganiserID = 4, RestaurantID = 2, ExactDateTime = DateTime.Now.AddDays(-100) },
                new Models.Reservation() { ID = 2, OrganiserID = 2, RestaurantID = 1, ExactDateTime = DateTime.Now.AddDays(-70) },
                new Models.Reservation() { ID = 3, OrganiserID = 3, RestaurantID = 4, ExactDateTime = DateTime.Now.AddDays(-54) },
                new Models.Reservation() { ID = 4, OrganiserID = 1, RestaurantID = 3, ExactDateTime = DateTime.Now.AddDays(-27) },
            ];

        public List<Models.Dinner> Dinners { get; set; } = [
                new Models.Dinner() { ID = 1, ReservationID = 4 },
                new Models.Dinner() { ID = 2, ReservationID = 1 },
                new Models.Dinner() { ID = 3, ReservationID = 2 },
                new Models.Dinner() { ID = 4, ReservationID = 3 },
            ];

        public List<Models.Member> Members { get; set; } = [
        new Models.Member() { ID = 1, Name = "Adrian Dick", IsFoundingFather = true },
            new Models.Member() { ID = 4, Name = "Matt Moore", IsFoundingFather = true },
            new Models.Member() { ID = 9, Name = "Dave Fowlie", IsFoundingFather = true },
            new Models.Member() { ID = 10, Name = "Francis Lings", IsFoundingFather = true },
            new Models.Member() { ID = 12, Name = "Rob MacLennan", IsFoundingFather = true },
            new Models.Member() { ID = 2, Name = "Brent Verry", IsFoundingFather = false },
            new Models.Member() { ID = 3, Name = "Brent Gladding", IsFoundingFather = false },
            new Models.Member() { ID = 5, Name = "Jason Mann", IsFoundingFather = false },
            new Models.Member() { ID = 6, Name = "Damian Coventry", IsFoundingFather = false },
            new Models.Member() { ID = 7, Name = "Martin Dick", IsFoundingFather = false },
            new Models.Member() { ID = 8, Name = "Justin MacLennan", IsFoundingFather = false },
            new Models.Member() { ID = 11, Name = "Chris Whitehead", IsFoundingFather = false },
            new Models.Member() { ID = 13, Name = "Kurt Kelsall", IsFoundingFather = false },
            new Models.Member() { ID = 14, Name = "Matt Wilson-Vogler", IsFoundingFather = false },
            new Models.Member() { ID = 15, Name = "Stu Macferson", IsFoundingFather = false },
            new Models.Member() { ID = 16, Name = "Philip Hunt", IsFoundingFather = false },
            new Models.Member() { ID = 17, Name = "Ken Nicod", IsFoundingFather = false },
            new Models.Member() { ID = 18, Name = "Scott Robinson", IsFoundingFather = false },
            new Models.Member() { ID = 19, Name = "Kenny Stuart", IsFoundingFather = false },
            new Models.Member() { ID = 20, Name = "Jason Armstrong", IsFoundingFather = false },
            new Models.Member() { ID = 21, Name = "Greg Long", IsFoundingFather = false },
            new Models.Member() { ID = 22, Name = "Chris Stephens", IsFoundingFather = false },
            new Models.Member() { ID = 23, Name = "Gary Spalding", IsFoundingFather = false },
            new Models.Member() { ID = 24, Name = "Kareem Kader", IsFoundingFather = false },
            new Models.Member() { ID = 25, Name = "Hamish Graham", IsFoundingFather = false },
            new Models.Member() { ID = 26, Name = "Elvin Maharaj", IsFoundingFather = false },
            new Models.Member() { ID = 27, Name = "Daryl Blake", IsFoundingFather = false },
            new Models.Member() { ID = 28, Name = "Paul James", IsFoundingFather = false },
            new Models.Member() { ID = 29, Name = "Davey Goode", IsFoundingFather = false },
            new Models.Member() { ID = 30, Name = "Peter Wakely", IsFoundingFather = false },
        ];

        public class DinnerInfo
        {
            public string RestaurantName { get; set; } = string.Empty;
            public string RestaurantStreetAddress { get; set; } = string.Empty;
            public string Organiser { get; set; } = string.Empty;
            public DateTime When { get; set; }
        }

        private List<DinnerInfo>? _dinnerInfos = null;

        public List<DinnerInfo> DinnerInfos
        {
            get
            {
                _dinnerInfos ??= Build();
                return _dinnerInfos;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private List<DinnerInfo> Build()
        {
            List<DinnerInfo> rv = [];
            foreach (var dinner in Dinners)
            {
                var reservation = Reservations.First(x => x.ID == dinner.ReservationID);
                var restaurant = Restaurants.First(x => x.ID == reservation.RestaurantID);
                var organiser = Members.First(x => x.ID == reservation.OrganiserID);

                rv.Add(new DinnerInfo
                {
                    RestaurantName = restaurant.Name,
                    RestaurantStreetAddress = restaurant.StreetAddress ?? "",
                    Organiser = organiser.Name,
                    When = reservation.ExactDateTime,
                });
            }
            return rv;
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

        public async void OnAddMember(object sender, EventArgs e)
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
    }
}
