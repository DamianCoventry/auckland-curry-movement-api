using acm_mobile_app.Services;
using acm_mobile_app.Views;

namespace acm_mobile_app
{
    public partial class AppShell : Shell
    {
        private readonly IAcmService _acmService;

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("add_club", typeof(AddClub));
            Routing.RegisterRoute("edit_club", typeof(EditClub));

            Routing.RegisterRoute("add_exemption", typeof(AddExemption));
            Routing.RegisterRoute("edit_exemption", typeof(EditExemption));

            Routing.RegisterRoute("add_level", typeof(AddLevel));
            Routing.RegisterRoute("edit_level", typeof(EditLevel));

            Routing.RegisterRoute("edit_member", typeof(EditMember));
            Routing.RegisterRoute("select_1_member", typeof(SelectOneMember));
            Routing.RegisterRoute("select_0_1_or_more_members", typeof(SelectZeroOneOrMoreMembers));

            Routing.RegisterRoute("add_restaurant", typeof(AddRestaurant));
            Routing.RegisterRoute("edit_restaurant", typeof(EditRestaurant));

            Routing.RegisterRoute("edit_reservation", typeof(EditReservation));

            Routing.RegisterRoute("about", typeof(About));

            _acmService = new AcmService();
        }

        public IAcmService AcmService { get { return _acmService; } }
    }
}
