namespace auckland_curry_movement_api.Models
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
        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }
        public virtual ICollection<Club>? Clubs { get; set; }
        public virtual ICollection<Dinner>? Dinners { get; set; }
    }
}
