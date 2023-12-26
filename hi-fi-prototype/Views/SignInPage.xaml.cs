namespace hi_fi_prototype.Views;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();
	}

    private async void SignInButton_Clicked(object sender, EventArgs e)
    {
        if (IsAuditor.IsChecked)
            await Shell.Current.GoToAsync("//manage_clubs");
        else
            await Shell.Current.GoToAsync("//membership_home");
    }
}