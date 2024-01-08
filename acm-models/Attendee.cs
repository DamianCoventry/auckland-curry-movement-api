namespace acm_models
{
    public class Attendee
    {
        public int? ID { get; set; }
        public int DinnerID { get; set; }
        public Dinner? Dinner { get; set; }
        public int MembershipID { get; set; }
        public Membership? Membership { get; set; }
        public int LevelID { get; set; }
        public Level? Level { get; set; }
        public bool IsSponsor { get; set; }
        public bool IsInductee { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
    }
}
