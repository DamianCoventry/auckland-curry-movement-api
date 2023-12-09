using acm_mobile_app.Services;
using System.Collections.ObjectModel;

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

            SigningInIndicator.IsRunning = true;
            SignInResult.Text = "Signing in...";
            await Task.Delay(150);

            bool signedIn = await AcmService.SignIn();
            await Task.Delay(150);

            SignInResult.Text = signedIn ? "Signed in" : "Failed to sign in";
            Token.Text = AcmService.AccessToken;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Close");
        }
        finally
        {
            SigningInIndicator.IsRunning = false;
        }
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }
}
