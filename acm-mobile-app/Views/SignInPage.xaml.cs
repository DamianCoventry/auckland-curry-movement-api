using acm_mobile_app.Services;

namespace acm_mobile_app.Views;

public partial class SignInPage : ContentPage
{
    public SignInPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public async void OnClickSignIn(object sender, EventArgs e)
    {
        try
        {
            if (SigningInIndicator.IsRunning)
                return;

            SignInButton.IsEnabled = false;
            SigningInIndicator.IsRunning = true;
            SignInResult.Text = "Signing in...";
            await Task.Delay(250);

            bool signedIn = await AcmService.SignIn();
            await Task.Delay(250);

            SignInResult.Text = signedIn ? "Signed in" : "Failed to sign in";
            SigningInIndicator.IsRunning = false;
            await Task.Delay(1500);

            await Shell.Current.GoToAsync("//home");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Close");
            SignInButton.IsEnabled = true;
        }
        finally
        {
            SigningInIndicator.IsRunning = false;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SignInButton.IsEnabled = true;
        SigningInIndicator.IsRunning = false;
        SignInResult.Text = string.Empty;
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }
}
