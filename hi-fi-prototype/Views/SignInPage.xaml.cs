using hi_fi_prototype.Services;

namespace hi_fi_prototype.Views
{
	public partial class SignInPage : ContentPage
	{
		public SignInPage()
		{
			InitializeComponent();
		}

		private static IAcmService AcmService
		{
			get { return ((AppShell)Shell.Current).AcmService; }
		}

		private async void SignInButton_Clicked(object sender, EventArgs e)
		{
            try
            {
			    if (SigningInProgress.IsVisible)
				    return;
                IsAuditor.IsEnabled = false;
                SignInButton.IsEnabled = false;
                SigningInProgress.IsVisible = true;
                SigningInIndicator.IsRunning = true;
                SigningInIndicator.IsVisible = true;
                SignInResult.Text = "Signing in...";
                await Task.Delay(250);

                bool success = await AcmService.SignIn();

                SignInResult.Text = success ? "Signed in" : "Failed to sign in";
                SigningInIndicator.IsRunning = false;
                SigningInIndicator.IsVisible = false;
                await Task.Delay(1500);

                if (IsAuditor.IsChecked) // Temporary
                    await Shell.Current.GoToAsync("//manage_clubs");
                else
                    await Shell.Current.GoToAsync("//manage_dinners");
            }
            catch (Exception ex)
		    {
                await DisplayAlert("Error", ex.Message, "Close");
            }
            finally
		    {
                IsAuditor.IsEnabled = true;
                SignInButton.IsEnabled = true;
                SigningInProgress.IsVisible = false;
            }
        }
	}
}
