using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Attendee : INotifyPropertyChanged
    {
        static public Attendee? FromModel(acm_models.AttendeeStats? model)
        {
            if (model == null || model.Attendee == null) return null;
            return new Attendee()
            {
                _id = model.Attendee.ID,
                _dinnerID = model.Attendee.DinnerID,
                _dinner = Dinner.FromModel(model.Attendee.Dinner),
                _memberID = model.Attendee.MemberID,
                _member = Member.FromModel(model.Attendee.Member),
                _levelID = model.Attendee.LevelID,
                _level = Level.FromModel(model.Attendee.Level),
                _isSponsor = model.Attendee.IsSponsor,
                _isInductee = model.Attendee.IsInductee,
                _nthAttendance = model.NthAttendance,
                _isExemptionUsed = model.IsExemptionUsed,
                _isReceivedViolation = model.IsReceivedViolation,
                _isFoundingFather = model.IsFoundingFather,
            };
        }

        static public Attendee? FromModel(acm_models.Attendee? model)
        {
            if (model == null) return null;
            return new Attendee()
            {
                _id = model.ID,
                _dinnerID = model.DinnerID,
                _dinner = Dinner.FromModel(model.Dinner),
                _memberID = model.MemberID,
                _member = Member.FromModel(model.Member),
                _levelID = model.LevelID,
                _level = Level.FromModel(model.Level),
                _isSponsor = model.IsSponsor,
                _isInductee = model.IsInductee,
            };
        }

        private int? _id;
        private int _dinnerID;
        private Dinner? _dinner;
        private int _memberID;
        private Member? _member;
        private int _levelID;
        private Level? _level;
        private bool _isSponsor;
        private bool _isInductee;
        private string _nthAttendance = string.Empty;
        private bool _isExemptionUsed;
        private bool _isReceivedViolation;
        private bool _isFoundingFather;

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

        public int DinnerID
        {
            get => _dinnerID;
            set
            {
                if (_dinnerID != value)
                {
                    _dinnerID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DinnerID)));
                }
            }
        }

        public Dinner? Dinner
        {
            get => _dinner;
            set
            {
                if (_dinner != value)
                {
                    _dinner = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dinner)));
                }
            }
        }

        public int MemberID
        {
            get => _memberID;
            set
            {
                if (_memberID != value)
                {
                    _memberID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberID)));
                }
            }
        }

        public Member? Member
        {
            get => _member;
            set
            {
                if (_member != value)
                {
                    _member = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
                }
            }
        }

        public int LevelID
        {
            get => _levelID;
            set
            {
                if (_levelID != value)
                {
                    _levelID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LevelID)));
                }
            }
        }

        public Level? Level
        {
            get => _level;
            set
            {
                if (_level != value)
                {
                    _level = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Level)));
                }
            }
        }

        public bool IsSponsor
        {
            get => _isSponsor;
            set
            {
                if (_isSponsor != value)
                {
                    _isSponsor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSponsor)));
                }
            }
        }

        public bool IsInductee
        {
            get => _isInductee;
            set
            {
                if (_isInductee != value)
                {
                    _isInductee = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsInductee)));
                }
            }
        }

        // TODO: How to make this programatic?
        public bool IsAuditor { get => Member != null && Member.Name == "Damian Coventry" && Member.ID == 6; }

        // TODO: How to make this programatic?
        public bool IsGuru { get => LevelID == 2; }
        public bool IsMaharaja { get => LevelID == 3; }

        public string NthAttendance { get => _nthAttendance; }
        public bool IsExemptionUsed { get => _isExemptionUsed; }
        public bool IsReceivedViolation { get => _isReceivedViolation; }
        public bool IsFoundingFather { get => _isFoundingFather; }

        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
