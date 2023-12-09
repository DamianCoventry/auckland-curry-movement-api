using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

public partial class ManageLevels : ContentPage
{
    private const int PAGE_SIZE = 10;
    private int _page = 0;
    private int _totalPages = 0;

    public ManageLevels()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public int CurrentPage { get { return _page + 1; } }

    public ObservableCollection<Level> Levels { get; set; } = [];

    public void OnClickRefreshData(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
    }

    public async void OnClickAdd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("edit_level");
    }

    public async void OnClickModify(object sender, EventArgs e)
    {
        if (LevelListView.SelectedItem == null)
        {
            var toast = Toast.Make("Select a level first", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        await Shell.Current.GoToAsync("edit_level");
    }

    public async void OnClickDelete(object sender, EventArgs e)
    {
        if (LevelListView.SelectedItem == null)
        {
            var toast = Toast.Make("Select a level first", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        if (await DisplayAlert("Delete Level", "Are you sure you want to delete the selected level?", "Yes", "No"))
        {
            // TODO: actually delete the level
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

            var levels = await AcmService.ListLevelsAsync(_page * PAGE_SIZE, PAGE_SIZE);
            _totalPages = levels.PageItems == null ? 0 : levels.TotalPages;

            await Task.Delay(150);

            LevelListView.SelectedItem = null;
            Levels.Clear();

            if (levels.PageItems != null)
            {
                foreach (var model in levels.PageItems)
                {
                    var x = Level.FromModel(model);
                    if (x != null)
                        Levels.Add(x);
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
}
