namespace auckland_curry_movement_api.Models
{
    public class Attendee
    {
        public int? ID { get; set; }
        public int DinnerID { get; set; }
        public Dinner? Dinner { get; set; }
        public int MemberID { get; set; }
        public Member? Member { get; set; }
        public int LevelID { get; set; }
        public Level? Level { get; set; }
        public bool IsSponsor { get; set; }
        public bool IsInductee { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
    }
}
