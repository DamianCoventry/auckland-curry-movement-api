namespace acm_models
{
    public class Notification
    {
        public int? ID { get; set; }
        public DateTime Date { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;

        public int? AttendeeID { get; set; }
        public Attendee? Attendee { get; set; }
        public int? ClubID { get; set; }
        public Club? Club { get; set; }
        public int? DinnerID { get; set; }
        public Dinner? Dinner { get; set; }
        public int? ExemptionID { get; set; }
        public Exemption? Exemption { get; set; }
        public int? KotCID { get; set; }
        public KotC? KotC { get; set; }
        public int? LevelID { get; set; }
        public Level? Level { get; set; }
        public int? MemberID { get; set; }
        public Member? Member { get; set; }
        public int? ReservationID { get; set; }
        public Reservation? Reservation { get; set; }
        public int? RestaurantID { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int? RotYYear { get; set; }
        public RotY? RotY { get; set; }
        public int? ViolationID { get; set; }
        public Violation? Violation { get; set; }
    }
}
