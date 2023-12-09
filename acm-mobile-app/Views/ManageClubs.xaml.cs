using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views
{
    public partial class ManageClubs : ContentPage
    {
        private const int PAGE_SIZE = 10;
        private int _page = 0;
        private int _totalPages = 0;
        private bool _isDeleting = false;

        public ManageClubs()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int CurrentPage { get { return _page + 1; } }

        public ObservableCollection<Club> Clubs { get; set; } = [];

        public void OnClickRefreshData(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
        }

        public async void OnClickAdd(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_club", true);
        }

        public async void OnClickModify(object sender, EventArgs e)
        {
            if (ClubListView.SelectedItem == null)
            {
                var toast = Toast.Make("Select a club first", ToastDuration.Short, 14);
                await toast.Show();
                return;
            }

            if (ClubListView.SelectedItem is Club club)
            {
                Dictionary<string, object> parameters = new() { { "ClubID", club.ID ?? 0 }, { "ClubName", club.Name } };
                await Shell.Current.GoToAsync("edit_club", true, parameters);
            }
        }

        public async void OnClickDelete(object sender, EventArgs e)
        {
            if (ClubListView.SelectedItem == null)
            {
                var toast = Toast.Make("Select a club first", ToastDuration.Short, 14);
                await toast.Show();
                return;
            }
            if (await DisplayAlert("Delete Club", "Are you sure you want to delete the selected club?", "Yes", "No"))
            {
                MainThread.BeginInvokeOnMainThread(async () => { await DeleteSelectedItem(); });
            }
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

                var clubs = await AcmService.ListClubsAsync(_page * PAGE_SIZE, PAGE_SIZE);
                _totalPages = clubs.PageItems == null ? 0 : clubs.TotalPages;

                await Task.Delay(150);

                ClubListView.SelectedItem = null;
                Clubs.Clear();

                if (clubs.PageItems != null)
                {
                    foreach (var model in clubs.PageItems)
                    {
                        var x = Club.FromModel(model);
                        if (x != null)
                            Clubs.Add(x);
                    }
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

        private async Task DeleteSelectedItem()
        {
            try
            {
                if (_isDeleting || ClubListView.SelectedItem == null)
                    return;

                if (ClubListView.SelectedItem is not Club selectedClub || selectedClub.ID == null)
                    return;

                _isDeleting = true;
                await Task.Delay(150);

                // TODO: don't actually delete the club, archive it.
                await AcmService.DeleteClubAsync((int)selectedClub.ID);

                await RefreshListData();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Close");
            }
            finally
            {
                _isDeleting = false;
            }
        }
    }
}
