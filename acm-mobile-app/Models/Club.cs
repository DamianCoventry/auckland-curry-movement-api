namespace acm_mobile_app.Models
{
    public class Club
    {
        public int? ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }

        public int NumMembers { get { return Members == null ? 0 : Members.Count; } }

        public virtual ICollection<Member>? Members { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
