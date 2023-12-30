using hi_fi_prototype.Services;
using hi_fi_prototype.Views;

namespace hi_fi_prototype
{
    public partial class AppShell : Shell
    {
        private readonly IAcmService _acmService;

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

            Routing.RegisterRoute("add_reservation", typeof(AddReservationPage));
            Routing.RegisterRoute("edit_reservation", typeof(EditReservationPage));

            Routing.RegisterRoute("edit_dinner", typeof(EditDinnerPage));
            Routing.RegisterRoute("edit_member", typeof(EditMemberPage));

            _acmService = new AcmService();
        }

        public IAcmService AcmService { get { return _acmService; } }
    }
}
