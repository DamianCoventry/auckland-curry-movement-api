using hi_fi_prototype.Services;

namespace hi_fi_prototype.Views
{
    public partial class SignOutPage : ContentPage
    {
        public SignOutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Logo.Opacity = 0;
            Logo.FadeTo(1, 1000, Easing.CubicIn);

            Activity.Opacity = 0;
            Activity.FadeTo(1, 1000, Easing.CubicIn);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                AcmService.SignOut();

                await Task.Delay(1500);

                await Shell.Current.GoToAsync("//sign_in");
            });
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }
    }
}