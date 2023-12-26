using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.ViewModels;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Restaurant), "Restaurant")]
    public partial class EditRestaurantPage : ContentPage
    {
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

            RestaurantName.IsEnabled = false;
            RestaurantStreetAddress.IsEnabled = false;
            RestaurantSuburb.IsEnabled = false;
            RestaurantPhoneNumber.IsEnabled = false;
            AcceptChanges.IsEnabled = false;
            DiscardChanges.IsEnabled = false;
            SavingIndicator.IsRunning = true;
            SavingIndicator.IsVisible = true;
            await Task.Delay(1500); // Fake saving
            await Shell.Current.GoToAsync("..");
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
