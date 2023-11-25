namespace acm_mobile_app.Views;

public partial class EditExemption : ContentPage
{
	public EditExemption()
	{
		InitializeComponent();
        BindingContext = this;
    }

    async public void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ShortReason.Text))
        {
            // TODO: display a 'short reason is required' method
            return;
        }
        // TODO: actually add it
        await Shell.Current.GoToAsync("//manage_exemptions");
    }

    async public void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }
}