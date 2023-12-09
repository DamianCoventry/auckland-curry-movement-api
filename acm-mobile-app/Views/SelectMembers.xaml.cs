using acm_mobile_app.ViewModels;
using acm_mobile_app.Services;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

public partial class SelectMembers : ContentPage
{
    private const int PAGE_SIZE = 10;
    private int _page = 0;
    private int _totalPages = 0;

    public SelectMembers()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public int CurrentPage { get { return _page + 1; } }

    public List<SelectedMember> MasterListOfMembers { get; set; } = [];
    public ObservableCollection<SelectedMember> CurrentPageOfMembers { get; set; } = [];

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
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

    public async void OnClickOK(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new() { { "SelectedMembers", MasterListOfMembers } };
        await Shell.Current.GoToAsync("..", true, parameters);
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async Task RefreshListData()
    {
        try
        {
            if (IsRefreshing.IsRunning)
                return;

            IsRefreshing.IsRunning = true;
            await Task.Delay(150);

            // TODO: Get the club ID from somewhere

            var members = await AcmService.ListClubMembersAsync(1, _page * PAGE_SIZE, PAGE_SIZE);
            _totalPages = members.PageItems == null ? 0 : members.TotalPages;

            MergeReceivedPageIntoMasterList(members.PageItems);

            await Task.Delay(150);

            MemberListView.SelectedItem = null;
            CurrentPageOfMembers.Clear();

            if (members.PageItems != null)
            {
                foreach (var memberModel in members.PageItems)
                {
                    CurrentPageOfMembers.Add(new SelectedMember()
                    {
                        IsSelected = IsMemberSelected(memberModel),
                        Member = Member.FromModel(memberModel)
                    });
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Close");
        }
        finally
        {
            IsRefreshing.IsRunning = false;
        }
    }

    private void MergeReceivedPageIntoMasterList(List<acm_models.Member>? memberModels)
    {
        if (memberModels == null)
            return;
        foreach (var memberModel in memberModels)
        {
            if (!MasterListOfMembers.Any(x => x.Member.ID == memberModel.ID))
            {
                MasterListOfMembers.Add(new SelectedMember()
                {
                    IsSelected = false,
                    Member = Member.FromModel(memberModel)
                });
            }
        }
    }

    private bool IsMemberSelected(acm_models.Member checkMe)
    {
        return MasterListOfMembers.Any(x => x.Member.ID == checkMe.ID && x.IsSelected);
    }

    private void MemberListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is SelectedMember currentPageMember)
        {
            currentPageMember.IsSelected = !currentPageMember.IsSelected;

            var masterMember = MasterListOfMembers.Find(x => x.Member.ID == currentPageMember.Member.ID);
            if (masterMember != null)
                masterMember.IsSelected = currentPageMember.IsSelected;
        }
    }

    private void MemberSelectedCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        SynchroniseCurrentPageWithMasterList();
    }

    private void SynchroniseCurrentPageWithMasterList()
    {
        foreach (var item in CurrentPageOfMembers)
        {
            var member = MasterListOfMembers.Find(x => x.Member.ID == item.Member.ID);
            if (member != null)
                member.IsSelected = item.IsSelected;
        }
    }
}
