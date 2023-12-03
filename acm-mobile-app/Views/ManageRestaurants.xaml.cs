using acm_mobile_app.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

public partial class ManageRestaurants : ContentPage
{
    private const int PAGE_SIZE = 10;
    private int _page = 0;
    private int _numItemsReturnedLastTime = 0;

    public ManageRestaurants()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public int CurrentPage { get { return _page + 1; } }

    public ObservableCollection<Models.Restaurant> Restaurants { get; set; } = [];

    public void OnClickRefreshData(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
    }

    public async void OnClickAdd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("edit_restaurant");
    }

    public async void OnClickModify(object sender, EventArgs e)
    {
        if (RestaurantListView.SelectedItem == null)
        {
            var toast = Toast.Make("Select a restaurant first", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        await Shell.Current.GoToAsync("edit_restaurant");
    }

    public async void OnClickDelete(object sender, EventArgs e)
    {
        if (RestaurantListView.SelectedItem == null)
        {
            var toast = Toast.Make("Select a restaurant first", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        if (await DisplayAlert("Delete Restaurant", "Are you sure you want to delete the selected restaurant?", "Yes", "No"))
        {
            // TODO: actually delete the restaurant
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
        if (_numItemsReturnedLastTime >= PAGE_SIZE)
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

            var restaurants = await AcmService.ListRestaurantsAsync(_page * PAGE_SIZE, PAGE_SIZE);
            _numItemsReturnedLastTime = restaurants.Count;

            await Task.Delay(250);

            RestaurantListView.SelectedItem = null;
            Restaurants.Clear();
            foreach (var restaurant in restaurants)
            {
                if (restaurant.Name != "<Unknown>")
                    Restaurants.Add(restaurant);
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
