namespace acm_models
{
    public class Meal
    {
        public int ReservationID { get; set; }
        public int OrganiserID { get; set; }
        public string OrganiserName { get; set; } = string.Empty;
        public int RestaurantID { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime ExactDateTime { get; set; }
        public double? NegotiatedBeerPrice { get; set; }
        public double? NegotiatedBeerDiscount { get; set; }
        public bool IsAmnesty { get; set; }
        public int? DinnerID { get; set; }
        public double? CostPerPerson { get; set; }
        public int? NumBeersConsumed { get; set; }
    }
}
