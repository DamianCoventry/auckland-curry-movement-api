using acm_mobile_app.Services;

namespace acm_mobile_app.Views;

public partial class SignInPage : ContentPage
{
    public SignInPage()
    {
        InitializeComponent();
        BindingContext = this;
        Members = [ new Models.Member() { Name ="Test" } ];
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            SignInPanel.IsVisible = !AcmService.IsSignedIn;
            Spinner.IsRunning = false;
            ApiPanel.IsVisible = AcmService.IsSignedIn;
        }
        catch (Exception ex)
        {
            Spinner.IsRunning = false;
            DisplayAlert("Unexpected error", ex.Message, "Close");
        }
    }

    public async void OnClickSignIn(object sender, EventArgs e)
    {
        try
        {
            SignInPanel.IsVisible = false;
            Spinner.IsRunning = true;
            await AcmService.SignIn();
            Spinner.IsRunning = false;
            ApiPanel.IsVisible = true;
        }
        catch (Exception ex)
        {
            Spinner.IsRunning = false;
            await DisplayAlert("Unexpected error", ex.Message, "Close");
        }
    }

    public async void OnClickSendGetRequest(object sender, EventArgs e)
    {
        try
        {
            ApiPanel.IsVisible = false;
            MemberListView.IsRefreshing = true;
            Members = await AcmService.ListMembersAsync(0, 20);
            MemberListView.IsRefreshing = false;
            ApiPanel.IsVisible = true;
        }
        catch (Exception ex)
        {
            MemberListView.IsRefreshing = false;
            Spinner.IsRunning = false;
            await DisplayAlert("Unexpected error", ex.Message, "Close");
        }
    }

    public List<Models.Member> Members { get; set; }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }
}
