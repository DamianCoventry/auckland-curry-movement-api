namespace acm_models
{
    public class MemberStats
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CurrentLevelID { get; set; }
        public int IsFoundingFather { get; set; }
        public int MembershipCount { get; set; }
        public int DinnersAttendedCount { get; set; }
        public int ExemptionsAwardedCount { get; set; }
        public int ExemptionsReceivedCount { get; set; }
        public int KotCCount { get; set; }
        public int ReservationOrganiserCount { get; set; }
        public int RotYPresenterCount { get; set; }
        public int ViolationsAwardedCount { get; set; }
        public int ViolationsReceivedCount { get; set; }
    }
}
