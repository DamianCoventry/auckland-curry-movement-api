namespace acm_mobile_app.Views;

public partial class ManageLevels : ContentPage
{
	public ManageLevels()
	{
		InitializeComponent();
        BindingContext = this;
    }

    // TODO: fill this is dynamically
    public List<Models.Level> Levels { get; set; } = [
        new Models.Level() { ID = 1, RequiredAttendances = 0, Name = "Novice", Description = "The entry level for all members." },
        new Models.Level() { ID = 2, RequiredAttendances = 50, Name = "Guru", Description = "Wear the Golden Turban during the 50th curry dinner." },
        new Models.Level() { ID = 3, RequiredAttendances = 100, Name = "Maharaja", Description = "Wear the Golden Jacket during the 100th curry dinner." },
    ];

    async public void OnClickAdd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("edit_level");
    }

    async public void OnClickModify(object sender, EventArgs e)
    {
        if (LevelsView.SelectedItem == null)
        {
            // TODO: display a 'select a level' message
            return;
        }
        await Shell.Current.GoToAsync("edit_level");
    }

    async public void OnClickDelete(object sender, EventArgs e)
    {
        if (LevelsView.SelectedItem == null)
        {
            // TODO: display a 'select a level' message
            return;
        }
        if (await DisplayAlert("Delete Level", "Are you sure you want to delete the selected level?", "Yes", "No"))
        {
            // TODO: actually delete the level
        }
    }
}