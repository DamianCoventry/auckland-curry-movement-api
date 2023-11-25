namespace acm_mobile_app.Views;

public partial class EditMember : ContentPage
{
	public EditMember()
	{
		InitializeComponent();
        BindingContext = this;
    }

    async public void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MemberName.Text))
        {
            // TODO: display a 'name is required' method
            return;
        }
        if (string.IsNullOrWhiteSpace(Sponsor.Text))
        {
            // TODO: display a 'description is required' method
            return;
        }
        // TODO: actually add it
        await Shell.Current.GoToAsync("//manage_members");
    }

    async public void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }
}