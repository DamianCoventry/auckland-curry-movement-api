using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Member : INotifyPropertyChanged
    {
        private int? _id;
        private string _name = string.Empty;
        private int? _sponsorID;
        private int _currentLevelID;
        private Level? _currentLevel;
        private int _attendanceCount;

        static public Member FromModel(acm_models.Member member)
        {
            return new Member()
            {
                _id = member.ID,
                _name = member.Name,
                _sponsorID = member.SponsorID,
                _currentLevelID = member.CurrentLevelID,
                _currentLevel = Level.FromModel(member.CurrentLevel),
                _attendanceCount = member.AttendanceCount,
            };
        }

        public int? ID
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public int? SponsorID
        {
            get => _sponsorID;
            set
            {
                if (_sponsorID != value)
                {
                    _sponsorID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SponsorID)));
                }
            }
        }

        public int CurrentLevelID
        {
            get => _currentLevelID;
            set
            {
                if (_currentLevelID != value)
                {
                    _currentLevelID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLevelID)));
                }
            }
        }

        public Level? CurrentLevel
        {
            get => _currentLevel;
            set
            {
                if (_currentLevel != value)
                {
                    _currentLevel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLevel)));
                }
            }
        }

        public int AttendanceCount
        {
            get => _attendanceCount;
            set
            {
                if (_attendanceCount != value)
                {
                    _attendanceCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AttendanceCount)));
                }
            }
        }

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

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
