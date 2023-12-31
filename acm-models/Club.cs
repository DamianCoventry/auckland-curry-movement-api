﻿namespace acm_models
{
    public class Club
    {
        public int? ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }

        public ICollection<Membership>? Memberships { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
