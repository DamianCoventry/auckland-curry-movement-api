using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

public partial class ManageMembers : ContentPage
{
    private const int PAGE_SIZE = 10;
    private int _page = 0;
    private int _totalPages = 0;
    private bool _isDeleting = false;

    public ManageMembers()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public int CurrentPage { get { return _page + 1; } }

    public ObservableCollection<MemberStats> MemberStats { get; set; } = [];

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

        if (MemberListView.SelectedItem is MemberStats memberStats)
        {
            Member copy = new()
            {
                ID = memberStats.ID,
                Name = memberStats.Name,
            };

            Dictionary<string, object> parameters = new() { { "Member", copy } };
            await Shell.Current.GoToAsync("edit_member", true, parameters);
        }
    }

    public async void OnClickDelete(object sender, EventArgs e)
    {
        if (MemberListView.SelectedItem == null)
        {
            var toast = Toast.Make("Select a member first", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (await DisplayAlert("Delete MemberStats", "Are you sure you want to delete the selected member?", "Yes", "No"))
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

            // TODO: Get the club ID from somewhere
            var memberStats = await AcmService.ListClubMemberStatsAsync(1, _page * PAGE_SIZE, PAGE_SIZE);
            _totalPages = memberStats.PageItems == null ? 0 : memberStats.TotalPages;

            await Task.Delay(150);

            MemberListView.SelectedItem = null;
            MemberStats.Clear();

            if (memberStats.PageItems != null)
            {
                foreach (var model in memberStats.PageItems)
                {
                    var x = ViewModels.MemberStats.FromModel(model);
                    if (x != null)
                        MemberStats.Add(x);
                }

                if (MemberStats.Count > 0)
                    MemberListView.ScrollTo(MemberStats[0], ScrollToPosition.Start, true);
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
            if (_isDeleting || MemberListView.SelectedItem == null)
                return;

            if (MemberListView.SelectedItem is not MemberStats selectedMember || selectedMember.ID == null)
                return;

            _isDeleting = true;
            await Task.Delay(150);

            // TODO: don't actually delete the member, archive it.
            await AcmService.DeleteMemberAsync((int)selectedMember.ID);

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
