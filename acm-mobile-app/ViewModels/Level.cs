namespace acm_mobile_app.ViewModels
{
    public class Level
    {
        static public Level FromModel(acm_models.Level? level)
        {
            if (level == null) return null;
            return new Level()
            {
                ID = level.ID,
                RequiredAttendances = level.RequiredAttendances,
                Name = level.Name,
                Description = level.Description,
                IsArchived = level.IsArchived,
                ArchiveReason = level.ArchiveReason,
            };
        }

        public int? ID { get; set; }
        public int RequiredAttendances { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }

        public virtual ICollection<Attendee>? Attendees { get; set; }
        public virtual ICollection<Member>? Members { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
