using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Violation : INotifyPropertyChanged
    {
        static public Violation? FromModel(acm_models.Violation? model)
        {
            if (model == null) return null;
            return new Violation()
            {
                _id = model.ID,
                _dinnerID = model.DinnerID,
                _dinner = Dinner.FromModel(model.Dinner),
                _foundingFatherID = model.FoundingFatherID,
                _foundingFather = Member.FromModel(model.FoundingFather),
                _memberID = model.MemberID,
                _member = Member.FromModel(model.Member),
                _description = model.Description,
                _isIndianHotCurry = model.IsIndianHotCurry,
                _isReinduction = model.IsReinduction,
                _otherPunishment = model.OtherPunishment
            };
        }

        private int? _id;
        private int _dinnerID;
        private Dinner? _dinner;
        private int _foundingFatherID;
        private Member? _foundingFather;
        private int _memberID;
        private Member? _member;
        private string _description = string.Empty;
        private bool _isIndianHotCurry;
        private bool _isReinduction;
        private string? _otherPunishment;

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

        public int FoundingFatherID
        {
            get => _foundingFatherID;
            set
            {
                if (_foundingFatherID != value)
                {
                    _foundingFatherID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoundingFatherID)));
                }
            }
        }

        public Member? FoundingFather
        {
            get => _foundingFather;
            set
            {
                if (_foundingFather != value)
                {
                    _foundingFather = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoundingFather)));
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

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        public bool IsIndianHotCurry
        {
            get => _isIndianHotCurry;
            set
            {
                if (_isIndianHotCurry != value)
                {
                    _isIndianHotCurry = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsIndianHotCurry)));
                }
            }
        }

        public bool IsReinduction
        {
            get => _isReinduction;
            set
            {
                if (_isReinduction != value)
                {
                    _isReinduction = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsReinduction)));
                }
            }
        }

        public string? OtherPunishment
        {
            get => _otherPunishment;
            set
            {
                if (_otherPunishment != value)
                {
                    _otherPunishment = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OtherPunishment)));
                }
            }
        }

        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
