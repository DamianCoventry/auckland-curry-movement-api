namespace acm_models
{
    public class Reservation
    {
        public int? ID { get; set; }
        public int OrganiserID { get; set; }
        public Membership? Organiser { get; set; }
        public int RestaurantID { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime ExactDateTime { get; set; }
        public double? NegotiatedBeerPrice { get; set; }
        public double? NegotiatedBeerDiscount { get; set; }
        public bool IsAmnesty { get; set; }

        public Dinner? Dinner { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
