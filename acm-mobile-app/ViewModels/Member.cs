using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Member : INotifyPropertyChanged
    {
        static public Member? FromModel(acm_models.Member? model)
        {
            if (model == null) return null;
            return new Member()
            {
                _id = model.ID,
                _name = model.Name,
                _sponsorID = model.SponsorID,
                _sponsor = FromModel(model.Sponsor),
                _currentLevelID = model.CurrentLevelID,
                _currentLevel = Level.FromModel(model.CurrentLevel),
                _attendanceCount = model.AttendanceCount,
            };
        }

        private int? _id;
        private string _name = string.Empty;
        private int? _sponsorID;
        private Member? _sponsor;
        private int _currentLevelID;
        private Level? _currentLevel;
        private int _attendanceCount;
        public bool _isArchived;
        public string? _archiveReason = string.Empty;

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

        public Member? Sponsor
        {
            get => _sponsor;
            set
            {
                if (_sponsor != value)
                {
                    _sponsor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sponsor)));
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

        public bool IsArchived
        {
            get => _isArchived;
            set
            {
                if (_isArchived != value)
                {
                    _isArchived = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsArchived)));
                }
            }
        }

        public string? ArchiveReason
        {
            get => _archiveReason;
            set
            {
                if (_archiveReason != value)
                {
                    _archiveReason = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArchiveReason)));
                }
            }
        }

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

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
