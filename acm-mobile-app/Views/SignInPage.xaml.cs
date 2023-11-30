using acm_mobile_app.Services;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

public partial class SignInPage : ContentPage
{
    private const int PAGE_SIZE = 10;
    private int _page = 0;
    private int _numMembersReturnedLastTime = 0;

    public SignInPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public ObservableCollection<Models.Member> Members { get; set; } = [
        new Models.Member() { Name = "Allie", IsFoundingFather = true, CurrentLevelID = 1 },
        new Models.Member() { Name = "Richard", IsFoundingFather = true, CurrentLevelID = 1 },
        new Models.Member() { Name = "Jocelynn", IsFoundingFather = true, CurrentLevelID = 1 },
        new Models.Member() { Name = "Moshe", IsFoundingFather = true, CurrentLevelID = 1 },
        new Models.Member() { Name = "Alexander", IsFoundingFather = true, CurrentLevelID = 1 },
        new Models.Member() { Name = "Skylar", IsFoundingFather = true, CurrentLevelID = 1 },
        new Models.Member() { Name = "Noemi", IsFoundingFather = true, CurrentLevelID = 1 },
        new Models.Member() { Name = "Malcolm", IsFoundingFather = true, CurrentLevelID = 1 }
        ];

    public int CurrentPage { get { return _page + 1; } }

    public async void SignIn(object o, EventArgs e)
    {
        try
        {
            Spinner.IsRunning = true;
            await AcmService.SignIn();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Close");
        }
        finally
        {
            Spinner.IsRunning = false;
        }
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    public async void RefreshMemberData(object o, EventArgs e)
    {
        try
        {
            ClubMembers.BeginRefresh();

            var members = await AcmService.ListMembersAsync(_page* PAGE_SIZE, PAGE_SIZE);
            _numMembersReturnedLastTime = members.Count;

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
            ClubMembers.EndRefresh();
        }
    }

    public void OnPreviousPage(object o, EventArgs e)
    {
        if (_page > 0)
        {
            --_page;
            RefreshMemberData(o, e);
            CurrentPageNumber.Text = (1 + _page).ToString();
        }
    }

    public void OnNextPage(object o, EventArgs e)
    {
        if (_numMembersReturnedLastTime >= PAGE_SIZE)
        {
            ++_page;
            RefreshMemberData(o, e);
            CurrentPageNumber.Text = (1 + _page).ToString();
        }
    }
}
