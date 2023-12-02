namespace acm_mobile_app.Models
{
    public class PastDinner
    {
        public int? ID { get; set; }
        public int OrganiserID { get; set; }
        public string OrganiserName { get; set; } = string.Empty;
        public int RestaurantID { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public DateTime ExactDateTime { get; set; }
        public double? NegotiatedBeerPrice { get; set; }
        public double? NegotiatedBeerDiscount { get; set; }
        public double? CostPerPerson { get; set; }
        public int? NumBeersConsumed { get; set; }
        public int IsNewKotC { get; set; }
        public int IsFormerRotY { get; set; }
        public int IsCurrentRotY { get; set; }
        public int IsRulesViolation { get; set; }

        public bool NewKotC { get { return IsNewKotC != 0; } }
        public bool FormerRotY { get { return IsFormerRotY != 0; } }
        public bool CurrentRotY { get { return IsCurrentRotY != 0; } }
        public bool RulesViolation { get { return IsRulesViolation != 0; } }
    }
}
