namespace acm_models
{
    public class Dinner
    {
        public int? ID { get; set; }
        public int ReservationID { get; set; }
        public Reservation? Reservation { get; set; }
        public double? CostPerPerson { get; set; }
        public int? NumBeersConsumed { get; set; }
        public KotC? KotC { get; set; }

        public ICollection<Attendee>? Attendees { get; set; }
        public ICollection<Member>? Members { get; set; }
        public ICollection<Violation>? Violations { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
