namespace acm_models
{
    public class Membership
    {
        public int? ID { get; set; }
        public int MemberID { get; set; }
        public Member? Member { get; set; }
        public int ClubID { get; set; }
        public int? SponsorID { get; set; }
        public Membership? Sponsor { get; set; }
        public int LevelID { get; set; }
        public Level? Level { get; set; }
        public int AttendanceCount { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsFoundingFather { get; set; }
        public bool IsAuditor { get; set; }
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
        public ICollection<Membership>? Inductees { get; set; }
    }
}
