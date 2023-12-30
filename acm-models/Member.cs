namespace acm_models
{
    public class Member
    {
        public int? ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? SponsorID { get; set; }
        public Member? Sponsor { get; set; }
        public int CurrentLevelID { get; set; }
        public Level? CurrentLevel { get; set; }
        public int AttendanceCount { get; set; }
        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }

        public ICollection<Club>? Clubs { get; set; }
        public ICollection<Dinner>? Dinners { get; set; }
        public ICollection<Attendee>? Attendees { get; set; }
        public ICollection<Exemption>? ExemptionsGiven { get; set; }
        public ICollection<Exemption>? ExemptionsReceived { get; set; }
        public ICollection<KotC>? KotCs { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<RotY>? RotYs { get; set; }
        public ICollection<Violation>? ViolationsGiven { get; set; }
        public ICollection<Violation>? ViolationsReceived { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Member>? Inductees { get; set; }
    }
}
