using hi_fi_prototype.Views;

namespace hi_fi_prototype
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("manage_notifications", typeof(ManageNotificationsPage));

            Routing.RegisterRoute("add_club", typeof(AddClubPage));
            Routing.RegisterRoute("edit_club", typeof(EditClubPage));

            Routing.RegisterRoute("add_level", typeof(AddLevelPage));
            Routing.RegisterRoute("edit_level", typeof(EditLevelPage));

            Routing.RegisterRoute("add_restaurant", typeof(AddRestaurantPage));
            Routing.RegisterRoute("edit_restaurant", typeof(EditRestaurantPage));
        }
    }
}
