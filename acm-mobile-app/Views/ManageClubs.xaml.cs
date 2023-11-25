namespace acm_mobile_app.Views;

public partial class ManageClubs : ContentPage
{
	public ManageClubs()
	{
		InitializeComponent();
        BindingContext = this;
    }

    // TODO: fill this is dynamically
    public List<string> Clubs { get; set; } = [
        "Auckland Curry Movement",
        "Korma Chameleons",
        "Aloo-minati",
        "Shashlik our spiced nuts",
        "Vindaloosers",
        "Pilau Talkers",
    ];

    async public void OnClickAdd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("edit_club");
    }

    async public void OnClickModify(object sender, EventArgs e)
    {
        if (ClubListView.SelectedItem == null)
        {
            // TODO: display a 'select a club' message
            return;
        }
        await Shell.Current.GoToAsync("edit_club");
    }

    async public void OnClickDelete(object sender, EventArgs e)
    {
        if (ClubListView.SelectedItem == null)
        {
            // TODO: display a 'select a club' message
            return;
        }
        if (await DisplayAlert("Delete Club", "Are you sure you want to delete the selected club?", "Yes", "No"))
        {
            // TODO: actually delete the club
        }
    }
}
