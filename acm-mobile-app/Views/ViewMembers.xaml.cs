using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

[QueryProperty(nameof(ClubID), "ClubID")]
[QueryProperty(nameof(ClubName), "ClubName")]
public partial class ViewMembers : ContentPage
{
    private const int PAGE_SIZE = 10;
    private int _page = 0;
    private int _totalPages = 0;

    public ViewMembers()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public int ClubID { get; set; }
    public string ClubName { get; set; } = string.Empty;

    public int CurrentPage { get { return _page + 1; } }

    public ObservableCollection<Member> Members { get; set; } = [];

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
        ClubNameLabel.Text = ClubName;
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

            var members = await AcmService.ListClubMembersAsync(ClubID, _page * PAGE_SIZE, PAGE_SIZE);
            _totalPages = members.PageItems == null ? 0 : members.TotalPages;

            await Task.Delay(150);

            MemberListView.SelectedItem = null;
            Members.Clear();

            if (members.PageItems != null)
            {
                foreach (var model in members.PageItems)
                {
                    var x = Member.FromModel(model);
                    if (x != null)
                        Members.Add(x);
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