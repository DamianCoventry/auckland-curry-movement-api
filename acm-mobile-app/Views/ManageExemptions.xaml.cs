namespace acm_mobile_app.Views;

public partial class ManageExemptions : ContentPage
{
	public ManageExemptions()
	{
		InitializeComponent();
        BindingContext = this;
    }

    // TODO: fill this is dynamically
    public List<Models.Exemption> Exemptions { get; set; } = [
        new Models.Exemption() { ID = 1, ShortReason = "Boobs", LongReason = "Vince sent in a picture of boobs." },
        new Models.Exemption() { ID = 2, ShortReason = "Moved to Christchurch", LongReason = "Chris moved to Christchurch permanently." },
        new Models.Exemption() { ID = 3, ShortReason = "Moved to Australia", LongReason = "Chris moved to Australia permanently." },
    ];

    async public void OnClickAdd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("edit_exemption");
    }

    async public void OnClickModify(object sender, EventArgs e)
    {
        if (ExemptionsView.SelectedItem == null)
        {
            // TODO: display a 'select a exemption' message
            return;
        }
        await Shell.Current.GoToAsync("edit_exemption");
    }

    async public void OnClickDelete(object sender, EventArgs e)
    {
        if (ExemptionsView.SelectedItem == null)
        {
            // TODO: display a 'select a exemption' message
            return;
        }
        if (await DisplayAlert("Delete Exemption", "Are you sure you want to delete the selected exemption?", "Yes", "No"))
        {
            // TODO: actually delete the exemption
        }
    }
}