namespace acm_mobile_app.Models
{
    public class Member
    {
        public int? ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsFoundingFather { get; set; }
        public int? SponsorID { get; set; }
        public int CurrentLevelID { get; set; }
        public Level? CurrentLevel { get; set; }
        public int AttendanceCount { get; set; }

        public virtual ICollection<Club>? Clubs { get; set; }
        public virtual ICollection<Dinner>? Dinners { get; set; }
        public virtual ICollection<Attendee>? Attendees { get; set; }
        public virtual ICollection<Exemption>? ExemptionsGiven { get; set; }
        public virtual ICollection<Exemption>? ExemptionsReceived { get; set; }
        public virtual ICollection<KotC>? KotCs { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
        public virtual ICollection<RotY>? RotYs { get; set; }
        public virtual ICollection<Violation>? ViolationsGiven { get; set; }
        public virtual ICollection<Violation>? ViolationsReceived { get; set; }
        public ICollection<Notification>? Notifications { get; set; }

        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }
    }
}
