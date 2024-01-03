using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Restaurant), "Restaurant")]
    public partial class EditRestaurantPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private readonly RestaurantViewModel _restaurant = new();

        public EditRestaurantPage()
        {
            InitializeComponent();
            BindingContext = Restaurant;
        }

        public RestaurantViewModel Restaurant
        {
            get => _restaurant;
            set
            {
                _restaurant.ID = value.ID;
                _restaurant.Name = value.Name;
                _restaurant.StreetAddress = value.StreetAddress;
                _restaurant.Suburb = value.Suburb;
                _restaurant.PhoneNumber = value.PhoneNumber;
                _restaurant.IsArchived = value.IsArchived;
                _restaurant.ArchiveReason = value.ArchiveReason;
            }
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_restaurant.Name))
            {
                var toast = Toast.Make("A name for the restaurant is required");
                await toast.Show();
                return;
            }

            if (string.IsNullOrWhiteSpace(_restaurant.Suburb))
            {
                var toast = Toast.Make("A suburb for the restaurant is required");
                await toast.Show();
                return;
            }

            MainThread.BeginInvokeOnMainThread(async () => { await SendSaveRestaurantMessage(); });
        }

        private void ShowMessageSendingGui(bool show = true)
        {
            RestaurantName.IsEnabled = !show;
            RestaurantStreetAddress.IsEnabled = !show;
            RestaurantSuburb.IsEnabled = !show;
            RestaurantPhoneNumber.IsEnabled = !show;
            AcceptChanges.IsEnabled = !show;
            DiscardChanges.IsEnabled = !show;
            SavingIndicator.IsRunning = show;
            SavingIndicator.IsVisible = show;

        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task SendSaveRestaurantMessage()
        {
            bool saved = false;
            try
            {
                if (SavingIndicator.IsRunning)
                    return;

                ShowMessageSendingGui();

                var startTime = DateTime.Now;

                saved = await AcmService.UpdateRestaurantAsync(new acm_models.Restaurant()
                {
                    ID = Restaurant.ID,
                    Name = Restaurant.Name,
                    StreetAddress = Restaurant.StreetAddress,
                    Suburb = Restaurant.Suburb,
                    PhoneNumber = Restaurant.PhoneNumber,
                    IsArchived = Restaurant.IsArchived,
                    ArchiveReason = Restaurant.ArchiveReason,
                });

                var elapsed = DateTime.Now - startTime;
                if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                    await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);

                if (!saved)
                {
                    var toast = Toast.Make("Unable to add these data as a new restaurant");
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
