namespace auckland_curry_movement_api.Models
{
    public class Dinner
    {
        public int? ID { get; set; }
        public int ReservationID { get; set; }
        public Reservation? Reservation { get; set; }
        public double? CostPerPerson { get; set; }
        public double? NumBeersConsumed { get; set; }
        public KotC? KotC { get; set; }

        public virtual ICollection<Attendee>? Attendees { get; set; }
        public virtual ICollection<Member>? Members { get; set; }
        public virtual ICollection<Violation>? Violations { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
