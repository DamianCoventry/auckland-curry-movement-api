using acm_mobile_app.Services;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views
{
    public partial class ManageClubs : ContentPage
    {
        private const int PAGE_SIZE = 10;
        private int _page = 0;
        private int _numDinnersReturnedLastTime = 0;

        public ManageClubs()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int CurrentPage { get { return _page + 1; } }

        public ObservableCollection<Models.Club> Clubs { get; set; } = [];

        public void OnClickRefreshData(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
        }

        public async void OnClickAdd(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("edit_club");
        }

        public async void OnClickModify(object sender, EventArgs e)
        {
            if (ClubListView.SelectedItem == null)
            {
                // TODO: display a 'select a club' message
                return;
            }
            await Shell.Current.GoToAsync("edit_club");
        }

        public async void OnClickDelete(object sender, EventArgs e)
        {
            if (ClubListView.SelectedItem == null)
            {
                // TODO: display a 'select a club' message
                return;
            }
            if (await DisplayAlert("Delete Club", "Are you sure you want to delete the selected club?", "Yes", "No"))
            {
                // TODO: actually delete the club
            }
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

                var clubs = await AcmService.ListClubsAsync(_page * PAGE_SIZE, PAGE_SIZE);
                _numDinnersReturnedLastTime = clubs.Count;

                await Task.Delay(250);

                Clubs.Clear();
                foreach (var club in clubs)
                    Clubs.Add(club);
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
