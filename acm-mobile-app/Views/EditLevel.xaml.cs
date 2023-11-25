namespace acm_mobile_app.Views;

public partial class EditLevel : ContentPage
{
	public EditLevel()
	{
		InitializeComponent();
        BindingContext = this;
    }

    async public void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LevelName.Text))
        {
            // TODO: display a 'name is required' method
            return;
        }
        if (string.IsNullOrWhiteSpace(LevelDescription.Text))
        {
            // TODO: display a 'description is required' method
            return;
        }
        if (string.IsNullOrWhiteSpace(RequiredAttendances.Text))
        {
            // TODO: display a 'required attendances is required' method
            return;
        }
        // TODO: actually add it
        await Shell.Current.GoToAsync("//manage_level");
    }

    async public void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }
}
