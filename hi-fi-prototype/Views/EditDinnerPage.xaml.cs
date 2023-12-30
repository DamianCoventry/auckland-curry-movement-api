using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.ViewModels;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Dinner), "Dinner")]
    public partial class EditDinnerPage : ContentPage
    {
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

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
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
