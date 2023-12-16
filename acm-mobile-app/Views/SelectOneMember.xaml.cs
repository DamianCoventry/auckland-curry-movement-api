using acm_mobile_app.ViewModels;
using acm_mobile_app.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using acm_models;

namespace acm_mobile_app.Views;

[QueryProperty(nameof(ClubID), "ClubID")]
[QueryProperty(nameof(SelectedMember), "SelectedMember")]
public partial class SelectOneMember : ContentPage
{
    private const int PAGE_SIZE = 10;
    private int _page = 0;
    private int _totalPages = 0;

    private readonly SelectedMember _originalMember = new();
    private readonly SelectedMember _selectedMember = new();

    public SelectOneMember()
    {
        InitializeComponent();
        BindingContext = this;
        _returnValue = null;
    }

    public SelectOneMember(Dictionary<string, object> parameters, Action<SelectedMember> returnValue)
    {
        InitializeComponent();
        BindingContext = this;

        if (parameters.TryGetValue("ClubID", out object? clubID) && clubID != null)
            ClubID = (int)clubID;

        if (parameters.TryGetValue("SelectedMember", out object? selectedMember) && selectedMember != null)
        {
            var x = (SelectedMember)selectedMember;
            SelectedMember = new SelectedMember() { IsFoundingFather = x.IsFoundingFather, IsSelected = x.IsSelected, Member = x.Member };
        }

        _returnValue = returnValue;
    }

    public int CurrentPage { get { return _page + 1; } }
    public int ClubID { get; set; }
    private readonly Action<SelectedMember>? _returnValue;

    public ObservableCollection<SelectedMember> CurrentPageOfMembers { get; set; } = [];

    public SelectedMember SelectedMember
    {
        get => _selectedMember;
        set
        {
            _selectedMember.IsSelected = value.IsSelected;
            _selectedMember.IsFoundingFather = value.IsFoundingFather;
            _selectedMember.Member = value.Member;
        }
    }

    private SelectedMember OriginalMember
    {
        get => _originalMember;
        set
        {
            _originalMember.IsSelected = value.IsSelected;
            _originalMember.IsFoundingFather = value.IsFoundingFather;
            _originalMember.Member = value.Member;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        OriginalMember = new SelectedMember()
        {
            IsSelected = SelectedMember.IsSelected,
            IsFoundingFather = SelectedMember.IsFoundingFather,
            Member = SelectedMember.Member
        };

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
        if (!SelectedMember.IsSelected)
        {
            var toast = Toast.Make("Select a club member", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        _returnValue?.Invoke(new()
        {
            IsSelected = SelectedMember.IsSelected,
            IsFoundingFather = SelectedMember.IsFoundingFather,
            Member = new ViewModels.Member() { ID = SelectedMember.Member.ID, Name = SelectedMember.Member.Name },
        });
        await Navigation.PopAsync(true);
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        _returnValue?.Invoke(new()
        {
            IsSelected = OriginalMember.IsSelected,
            IsFoundingFather = OriginalMember.IsFoundingFather,
            Member = new ViewModels.Member() { ID = OriginalMember.Member.ID, Name = OriginalMember.Member.Name },
        });
        await Navigation.PopAsync(true);
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

            acm_models.PageOfData<acm_models.Member> members;
            if (_selectedMember.IsFoundingFather)
                members = await AcmService.ListClubFoundingFathersAsync(ClubID, _page * PAGE_SIZE, PAGE_SIZE);
            else
                members = await AcmService.ListClubMembersAsync(ClubID, _page * PAGE_SIZE, PAGE_SIZE);

            _totalPages = members.PageItems == null ? 0 : members.TotalPages;

            await Task.Delay(150);

            MemberListView.SelectedItem = null;
            CurrentPageOfMembers.Clear();

            if (members.PageItems != null)
            {
                foreach (var model in members.PageItems)
                {
                    var member = new SelectedMember()
                    {
                        IsSelected = model.ID == SelectedMember.Member.ID && SelectedMember.IsSelected,
                        Member = ViewModels.Member.FromModel(model) ?? throw new NullReferenceException(nameof(model)),
                    };
                    CurrentPageOfMembers.Add(member);

                    if (member.IsSelected)
                        MemberListView.SelectedItem = member;
                }

                if (CurrentPageOfMembers.Count > 0)
                    MemberListView.ScrollTo(CurrentPageOfMembers[0], ScrollToPosition.Start, true);
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

    private void MemberListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is SelectedMember item)
        {
            _selectedMember.IsSelected = true;
            _selectedMember.Member = item.Member;
        }
    }
}
