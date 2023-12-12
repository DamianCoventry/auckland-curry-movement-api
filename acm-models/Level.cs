namespace acm_models
{
    public class Level
    {
        public int? ID { get; set; }
        public int RequiredAttendances { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }

        public ICollection<Attendee>? Attendees { get; set; }
        public ICollection<Member>? Members { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
