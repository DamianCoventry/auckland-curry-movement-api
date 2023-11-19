namespace auckland_curry_movement_api.Models
{
    public class Attendee
    {
        public int? ID { get; set; }
        public int DinnerID { get; set; }
        public int MemberID { get; set; }
        public int LevelID { get; set; }
        public bool IsSponsor { get; set; }
        public bool IsInductee { get; set; }
    }
}
