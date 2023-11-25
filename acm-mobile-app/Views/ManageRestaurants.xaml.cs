namespace acm_mobile_app.Views;

public partial class ManageRestaurants : ContentPage
{
	public ManageRestaurants()
	{
		InitializeComponent();
        BindingContext = this;
    }

    // TODO: fill this is dynamically
    public List<Models.Restaurant> Restaurants { get; set; } = [
                new Models.Restaurant() { ID = 1, Name = "Little India", StreetAddress = "142 West Coast Road" },
                new Models.Restaurant() { ID = 2, Name = "Moo's Backyard", StreetAddress = "37 Albany Road" },
                new Models.Restaurant() { ID = 3, Name = "Satya South Indian", StreetAddress = "515 Sandringham Road" },
                new Models.Restaurant() { ID = 4, Name = "Kabana Indian Cuisine", StreetAddress = "598 New North Road" },
            ];

    async public void OnClickAdd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("edit_restaurant");
    }

    async public void OnClickModify(object sender, EventArgs e)
    {
        if (RestaurantsView.SelectedItem == null)
        {
            // TODO: display a 'select a restaurant' message
            return;
        }
        await Shell.Current.GoToAsync("edit_restaurant");
    }

    async public void OnClickDelete(object sender, EventArgs e)
    {
        if (RestaurantsView.SelectedItem == null)
        {
            // TODO: display a 'select a restaurant' message
            return;
        }
        if (await DisplayAlert("Delete Restaurant", "Are you sure you want to delete the selected restaurant?", "Yes", "No"))
        {
            // TODO: actually delete the restaurant
        }
    }
}