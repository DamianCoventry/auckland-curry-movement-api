namespace auckland_curry_movement_api.Models
{
    public class Violation
    {
        public int? ID { get; set; }
        public int DinnerID { get; set; }
        public int FoundingFatherID { get; set; }
        public int MemberID { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsIndianHotCurry { get; set; }
        public bool IsReinduction { get; set; }
        public string? OtherPunishment { get; set; }
    }
}
