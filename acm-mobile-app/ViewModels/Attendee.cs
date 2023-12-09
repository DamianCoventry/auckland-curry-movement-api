using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Attendee : INotifyPropertyChanged
    {
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

        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
