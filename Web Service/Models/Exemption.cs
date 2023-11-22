namespace auckland_curry_movement_api.Models
{
    public class Exemption
    {
        public int? ID { get; set; }
        public int FoundingFatherID { get; set; }
        public Member? FoundingFather { get; set; }
        public int MemberID { get; set; }
        public Member? Member { get; set; }
        public DateTime Date { get; set; }
        public string ShortReason { get; set; } = string.Empty;
        public string LongReason { get; set; } = string.Empty;
        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }

        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}
