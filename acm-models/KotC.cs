namespace acm_models
{
    public class KotC
    {
        public int? ID { get; set; }
        public int MemberID { get; set; }
        public Member? Member { get; set; }
        public int DinnerID { get; set; }
        public Dinner? Dinner { get; set; }
        public int NumChillisConsumed { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
    }
}
