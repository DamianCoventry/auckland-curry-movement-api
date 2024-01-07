using hi_fi_prototype.Services;

namespace hi_fi_prototype.Views
{
	public partial class SignInPage : ContentPage
	{
		public SignInPage()
		{
			InitializeComponent();

            SignIn.Activity = async (parameter) =>
            {
                return await AcmService.SignIn();
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            IsAuditorCheckBox.IsEnabled = true;

            Task.Run(StartAnimation);
        }

        private static IAcmService AcmService
		{
			get { return ((AppShell)Shell.Current).AcmService; }
		}

        private async void SignIn_ActivitySucceeded(object sender, EventArgs e)
        {
            if (IsAuditorCheckBox.IsChecked) // Temporary
                await Shell.Current.GoToAsync("//manage_clubs");
            else
                await Shell.Current.GoToAsync("//manage_dinners");
        }

        private async void SignIn_ActivityFailed(object sender, EventArgs e)
        {
            await DisplayAlert("Error", SignIn.FailureExceptionText, "Close");
        }

        private async Task StartAnimation()
        {
            Logo.Opacity = 0;
            Auditor.Opacity = 0;
            SignIn.Opacity = 0;
            double logoY = Logo.TranslationX;
            double auditorX = Auditor.TranslationX;
            double signInX = SignIn.TranslationX;
            Logo.TranslationY -= 500;
            Auditor.TranslationX -= 75;
            SignIn.TranslationX += 75;

            Easing easing = Easing.CubicOut;
            const uint translateTimeMs = 500;
            const uint fadeTimeMs = 1000;

            await Task.WhenAny([
                Logo.FadeTo(1, fadeTimeMs*2, easing),
                Logo.TranslateTo(Logo.TranslationX, logoY, translateTimeMs, easing),
            ]);

            await Task.WhenAny([
                Auditor.FadeTo(1, fadeTimeMs, easing),
                Auditor.TranslateTo(auditorX, Auditor.TranslationY, translateTimeMs/2, easing),
            ]);

            await Task.WhenAny([
                SignIn.FadeTo(1, fadeTimeMs, easing),
                SignIn.TranslateTo(signInX, SignIn.TranslationY, translateTimeMs/2, easing),
            ]);
        }
    }
}
