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

        public int CurrentPage { get { return _page + 1; } }

        public ObservableCollection<Models.PastDinner> PastDinners { get; set; } = [];

        public void OnClickRefreshData(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
        }

        public void OnClickAdd(object sender, EventArgs e)
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

        public void OnPreviousPage(object o, EventArgs e)
        {
            if (_page > 0)
            {
                --_page;
                MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
                CurrentPageNumber.Text = (1 + _page).ToString();
            }
        }

        public void OnNextPage(object o, EventArgs e)
        {
            if (_numDinnersReturnedLastTime >= PAGE_SIZE)
            {
                ++_page;
                MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
                CurrentPageNumber.Text = (1 + _page).ToString();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task RefreshListData()
        {
            try
            {
                if (IsRefreshingListData.IsRunning)
                    return;

                IsRefreshingListData.IsRunning = true;
                await Task.Delay(250);

                // TODO: Get the club ID from somewhere
                var dinners = await AcmService.ListClubPastDinnersAsync(1, _page * PAGE_SIZE, PAGE_SIZE);
                _numDinnersReturnedLastTime = dinners.Count;

                await Task.Delay(250);

                PastDinners.Clear();
                foreach (var dinner in dinners)
                    PastDinners.Add(dinner);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Close");
            }
            finally
            {
                IsRefreshingListData.IsRunning = false;
            }
        }
    }
}
