namespace acm_mobile_app.ViewModels
{
    public class RotY
    {
        public int? Year { get; set; }
        public int RestaurantID { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int NumVotes { get; set; }
        public double WinningScore { get; set; }
        public int? PresenterID { get; set; }
        public Member? Presenter { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
    }
}
