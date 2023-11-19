namespace auckland_curry_movement_api.Models
{
    public class Dinner
    {
        public int? ID { get; set; }
        public int ReservationID { get; set; }
        public double? CostPerPerson { get; set; }
        public double? NumBeersConsumed { get; set; }
        public virtual ICollection<Member>? Members { get; set; }
    }
}
