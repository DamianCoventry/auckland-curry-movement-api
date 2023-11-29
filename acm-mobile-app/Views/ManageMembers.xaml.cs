namespace acm_mobile_app.Views;

public partial class ManageMembers : ContentPage
{
	public ManageMembers()
	{
		InitializeComponent();
        BindingContext = this;
    }

    // TODO: fill this is dynamically
    public List<Models.Member> Members { get; set; } = [
        new Models.Member() { ID = 1, Name = "Adrian Dick", IsFoundingFather = true },
        new Models.Member() { ID = 2, Name = "Brent Verry", IsFoundingFather = false },
        new Models.Member() { ID = 3, Name = "Brent Gladding", IsFoundingFather = false },
        new Models.Member() { ID = 4, Name = "Matt Moore", IsFoundingFather = true },
        new Models.Member() { ID = 5, Name = "Jason Mann", IsFoundingFather = false },
        new Models.Member() { ID = 6, Name = "Damian Coventry", IsFoundingFather = false },
        new Models.Member() { ID = 7, Name = "Martin Dick", IsFoundingFather = false },
        new Models.Member() { ID = 8, Name = "Justin MacLennan", IsFoundingFather = false },
        new Models.Member() { ID = 9, Name = "Dave Fowlie", IsFoundingFather = true },
        new Models.Member() { ID = 10, Name = "Francis Lings", IsFoundingFather = true },
        new Models.Member() { ID = 11, Name = "Chris Whitehead", IsFoundingFather = false },
        new Models.Member() { ID = 12, Name = "Rob MacLennan", IsFoundingFather = true },
        new Models.Member() { ID = 13, Name = "Kurt Kelsall", IsFoundingFather = false },
        new Models.Member() { ID = 14, Name = "Matt Wilson-Vogler", IsFoundingFather = false },
        new Models.Member() { ID = 15, Name = "Stu Macferson", IsFoundingFather = false },
        new Models.Member() { ID = 16, Name = "Philip Hunt", IsFoundingFather = false },
        new Models.Member() { ID = 17, Name = "Ken Nicod", IsFoundingFather = false },
        new Models.Member() { ID = 18, Name = "Scott Robinson", IsFoundingFather = false },
        new Models.Member() { ID = 19, Name = "Kenny Stuart", IsFoundingFather = false },
        new Models.Member() { ID = 20, Name = "Jason Armstrong", IsFoundingFather = false },
        new Models.Member() { ID = 21, Name = "Greg Long", IsFoundingFather = false },
        new Models.Member() { ID = 22, Name = "Chris Stephens", IsFoundingFather = false },
        new Models.Member() { ID = 23, Name = "Gary Spalding", IsFoundingFather = false },
        new Models.Member() { ID = 24, Name = "Kareem Kader", IsFoundingFather = false },
        new Models.Member() { ID = 25, Name = "Hamish Graham", IsFoundingFather = false },
        new Models.Member() { ID = 26, Name = "Elvin Maharaj", IsFoundingFather = false },
        new Models.Member() { ID = 27, Name = "Daryl Blake", IsFoundingFather = false },
        new Models.Member() { ID = 28, Name = "Paul James", IsFoundingFather = false },
        new Models.Member() { ID = 29, Name = "Davey Goode", IsFoundingFather = false },
        new Models.Member() { ID = 30, Name = "Peter Wakely", IsFoundingFather = false },
    ];

    public async void OnClickAdd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("edit_member");
    }

    public async void OnClickModify(object sender, EventArgs e)
    {
        if (MembersView.SelectedItem == null)
        {
            // TODO: display a 'select a member' message
            return;
        }
        await Shell.Current.GoToAsync("edit_member");
    }

    public async void OnClickDelete(object sender, EventArgs e)
    {
        if (MembersView.SelectedItem == null)
        {
            // TODO: display a 'select a member' message
            return;
        }
        if (await DisplayAlert("Delete Member", "Are you sure you want to delete the selected member?", "Yes", "No"))
        {
            // TODO: actually delete the member
        }
    }
}