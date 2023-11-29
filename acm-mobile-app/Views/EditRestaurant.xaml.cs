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
            // TODO: display a 'name is required' method
            return;
        }
        if (string.IsNullOrWhiteSpace(Suburb.Text))
        {
            // TODO: display a 'suburb is required' method
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