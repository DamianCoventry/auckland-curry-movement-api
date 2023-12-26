namespace hi_fi_prototype.Views;

public partial class SignOutPage : ContentPage
{
	public SignOutPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(1500);
            await Shell.Current.GoToAsync("//sign_in");
        });
    }
}