namespace auckland_curry_movement_api.Models
{
    public class Reservation
    {
        public int? ID { get; set; }
        public int OrganiserID { get; set; }
        public int RestaurantID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime ExactDateTime { get; set; }
        public double? NegotiatedBeerPrice { get; set; }
        public double? NegotiatedBeerDiscount { get; set; }
    }
}
