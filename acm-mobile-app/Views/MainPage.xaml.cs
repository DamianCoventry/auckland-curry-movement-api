using acm_mobile_app.Services;
using System.Collections.ObjectModel;

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
            RefreshDinnerData(this, new EventArgs());
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

        public ObservableCollection<Models.Dinner> Dinners { get; set; } = [];

        public async void RefreshDinnerData(object o, EventArgs e)
        {
            try
            {
                ClubDinners.BeginRefresh();

                var members = await AcmService.ListDinnersAsync(_page * PAGE_SIZE, PAGE_SIZE);
                _numDinnersReturnedLastTime = members.Count;

                Dinners.Clear();
                foreach (var member in members)
                    Dinners.Add(member);
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
                RefreshDinnerData(o, e);
                CurrentPageNumber.Text = (1 + _page).ToString();
            }
        }

        public void OnNextPage(object o, EventArgs e)
        {
            if (_numDinnersReturnedLastTime >= PAGE_SIZE)
            {
                ++_page;
                RefreshDinnerData(o, e);
                CurrentPageNumber.Text = (1 + _page).ToString();
            }
        }
    }
}
