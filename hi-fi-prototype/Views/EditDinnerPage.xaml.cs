using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Dinner), "Dinner")]
    public partial class EditDinnerPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private readonly DinnerViewModel _dinner = new();

        public EditDinnerPage()
        {
            InitializeComponent();
            BindingContext = Dinner;
        }

        public DinnerViewModel Dinner
        {
            get => _dinner;
            set
            {
                _dinner.ID = value.ID;
                _dinner.ReservationID = value.ReservationID;
                _dinner.Reservation = value.Reservation;
                _dinner.CostPerPerson = value.CostPerPerson;
                _dinner.NumBeersConsumed = value.NumBeersConsumed;
            }
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { await SendSaveDinnerMessage(); });
        }

        private void ShowMessageSendingGui(bool show = true)
        {
            AcceptChanges.IsEnabled = !show;
            DiscardChanges.IsEnabled = !show;
            SavingIndicator.IsRunning = show;
            SavingIndicator.IsVisible = show;
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task SendSaveDinnerMessage()
        {
            bool saved = false;
            try
            {
                if (SavingIndicator.IsRunning)
                    return;

                ShowMessageSendingGui();

                var startTime = DateTime.Now;

                var model = new acm_models.Dinner()
                {
                    ID = Dinner.ID,
                    ReservationID = Dinner.ReservationID,
                    CostPerPerson = Dinner.CostPerPerson,
                    NumBeersConsumed = Dinner.NumBeersConsumed,
                };

                if (Dinner.Reservation != null)
                    model.Reservation = new acm_models.Reservation() { ID = Dinner.Reservation.ID };

                saved = await AcmService.UpdateDinnerAsync(model);

                var elapsed = DateTime.Now - startTime;
                if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                    await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);

                if (!saved)
                {
                    var toast = Toast.Make("Unable to add these data as a new reservation");
                    await toast.Show();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                ShowMessageSendingGui(false);
            }

            if (saved)
                await Shell.Current.GoToAsync("..");
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
