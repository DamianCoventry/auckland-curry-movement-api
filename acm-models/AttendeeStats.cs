namespace acm_models
{
    public class AttendeeStats
    {
        public Attendee? Attendee { get; set; }
        public string NthAttendance { get; set; } = string.Empty;
        public bool IsExemptionUsed { get; set; }
        public bool IsReceivedViolation { get; set; }
        public bool IsFoundingFather { get; set; }
    }
}
