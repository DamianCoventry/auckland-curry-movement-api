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
            // TODO: display a 'name is required' method
            return;
        }
        if (string.IsNullOrWhiteSpace(Restaurant.Text))
        {
            // TODO: display a 'suburb is required' method
            return;
        }
        // TODO: actually add it
        await Shell.Current.GoToAsync("//home");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }
}