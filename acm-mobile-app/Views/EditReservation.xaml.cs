using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

public partial class EditReservation : ContentPage
{
	public EditReservation()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Organiser.Text))
        {
            var toast = Toast.Make("An organiser for the reservation is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        if (string.IsNullOrWhiteSpace(Restaurant.Text))
        {
            var toast = Toast.Make("A restaurant for the reservation is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        // TODO: actually add it
        await Shell.Current.GoToAsync("//home");
    }
}