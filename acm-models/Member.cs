namespace acm_models
{
    public class Member
    {
        public int? ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }
    }
}
