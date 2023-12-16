using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using System.Collections.ObjectModel;

namespace acm_mobile_app
{
    public partial class MainPage : ContentPage
    {
        private const int PAGE_SIZE = 10;
        private int _page = 0;
        private int _totalPages = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int CurrentPage { get { return _page + 1; } }

        public ObservableCollection<PastDinner> PastDinners { get; set; } = [];

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
            await Shell.Current.GoToAsync("add_restaurant");
        }

        public async void OnAddDinner(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("edit_member");
        }

        public async void NavigateToEditLevel(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_level");
        }

        public async void NavigateToEditClub(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_club", true);
        }

        public void OnClickFirst(object sender, EventArgs e)
        {
            if (_page > 0)
            {
                _page = 0;
                MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
                CurrentPageNumber.Text = (1 + _page).ToString();
            }
        }

        public void OnClickPrevious(object sender, EventArgs e)
        {
            if (_page > 0)
            {
                --_page;
                MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
                CurrentPageNumber.Text = (1 + _page).ToString();
            }
        }

        public void OnClickNext(object sender, EventArgs e)
        {
            if (_page < _totalPages - 1)
            {
                ++_page;
                MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
                CurrentPageNumber.Text = (1 + _page).ToString();
            }
        }

        public void OnClickLast(object sender, EventArgs e)
        {
            if (_page < _totalPages - 1)
            {
                _page = _totalPages - 1;
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
                await Task.Delay(150);

                // TODO: Get the club ID from somewhere
                var dinners = await AcmService.ListClubPastDinnersAsync(1, _page * PAGE_SIZE, PAGE_SIZE);
                _totalPages = dinners.PageItems == null ? 0 : dinners.TotalPages;

                await Task.Delay(150);

                DinnerListView.SelectedItem = null;
                PastDinners.Clear();

                if (dinners.PageItems != null)
                {
                    foreach (var model in dinners.PageItems)
                    {
                        var x = PastDinner.FromModel(model);
                        if (x != null)
                            PastDinners.Add(x);
                    }

                    if (PastDinners.Count > 0)
                        DinnerListView.ScrollTo(PastDinners[0], ScrollToPosition.Start, true);
                }
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
