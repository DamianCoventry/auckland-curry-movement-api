using acm_mobile_app.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

public partial class ManageMembers : ContentPage
{
    private const int PAGE_SIZE = 10;
    private int _page = 0;
    private int _numItemsReturnedLastTime = 0;

    public ManageMembers()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public int CurrentPage { get { return _page + 1; } }

    public ObservableCollection<Models.Member> Members { get; set; } = [];

    public void OnClickRefreshData(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
    }

    public async void OnClickAdd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("edit_member");
    }

    public async void OnClickModify(object sender, EventArgs e)
    {
        if (MemberListView.SelectedItem == null)
        {
            var toast = Toast.Make("Select a member first", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        await Shell.Current.GoToAsync("edit_member");
    }

    public async void OnClickDelete(object sender, EventArgs e)
    {
        if (MemberListView.SelectedItem == null)
        {
            var toast = Toast.Make("Select a member first", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        if (await DisplayAlert("Delete Member", "Are you sure you want to delete the selected member?", "Yes", "No"))
        {
            // TODO: actually delete the member
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

            var members = await AcmService.ListMembersAsync(_page * PAGE_SIZE, PAGE_SIZE);
            _numItemsReturnedLastTime = members.Count;

            await Task.Delay(250);

            MemberListView.SelectedItem = null;
            Members.Clear();
            foreach (var member in members)
                Members.Add(member);
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
