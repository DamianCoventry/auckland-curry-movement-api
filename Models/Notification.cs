namespace auckland_curry_movement_api.Models
{
    public class Notification
    {
        public int? ID { get; set; }
        public DateTime Date { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public int? AttendeeID { get; set; }
        public int? ClubID { get; set; }
        public int? DinnerID { get; set; }
        public int? ExemptionID { get; set; }
        public int? KotCID { get; set; }
        public int? LevelID { get; set; }
        public int? MemberID { get; set; }
        public int? ReservationID { get; set; }
        public int? RestaurantID { get; set; }
        public int? RotYID { get; set; }
        public int? ViolationID { get; set; }
    }
}
