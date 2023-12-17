using acm_mobile_app.Services;

namespace acm_mobile_app.Views;

public partial class SignOutPage : ContentPage
{
	public SignOutPage()
	{
		InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            AcmService.SignOut();

            await Task.Delay(1500);

            await Shell.Current.GoToAsync("//signin");
        });
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }
}