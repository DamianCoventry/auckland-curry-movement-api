using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

public partial class EditRestaurant : ContentPage
{
	public EditRestaurant()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RestaurantName.Text))
        {
            var toast = Toast.Make("A name for the restaurant is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        if (string.IsNullOrWhiteSpace(Suburb.Text))
        {
            var toast = Toast.Make("A suburb for the restaurant is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        // TODO: actually add it
        await Shell.Current.GoToAsync("//manage_restaurants");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }
}