using acm_mobile_app.Views;

namespace acm_mobile_app
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("edit_club", typeof(EditClub));
            Routing.RegisterRoute("edit_exemption", typeof(EditExemption));
            Routing.RegisterRoute("edit_level", typeof(EditLevel));
            Routing.RegisterRoute("edit_member", typeof(EditMember));
            Routing.RegisterRoute("edit_restaurant", typeof(EditRestaurant));
        }
    }
}
