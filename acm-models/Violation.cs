namespace acm_models
{
    public class Violation
    {
        public int? ID { get; set; }
        public int DinnerID { get; set; }
        public Dinner? Dinner { get; set; }
        public int FoundingFatherID { get; set; }
        public Membership? FoundingFather { get; set; }
        public int MemberID { get; set; }
        public Membership? Membership { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsIndianHotCurry { get; set; }
        public bool IsReinduction { get; set; }
        public string? OtherPunishment { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
    }
}
