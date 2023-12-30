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