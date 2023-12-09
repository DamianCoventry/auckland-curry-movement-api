using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Exemption : INotifyPropertyChanged
    {
        static public Exemption? FromModel(acm_models.Exemption? model)
        {
            if (model == null) return null;
            return new Exemption()
            {
                _id = model.ID,
                _foundingFatherID = model.FoundingFatherID,
                _foundingFather = Member.FromModel(model.FoundingFather),
                _memberID = model.MemberID,
                _member = Member.FromModel(model.Member),
                _date = model.Date,
                _shortReason = model.ShortReason,
                _longReason = model.LongReason,
                _isArchived = model.IsArchived,
                _archiveReason = model.ArchiveReason,
            };
        }

        private int? _id;
        private int _foundingFatherID;
        private Member? _foundingFather;
        private int _memberID;
        private Member? _member;
        private DateTime _date;
        private string _shortReason = string.Empty;
        private string _longReason = string.Empty;
        private bool _isArchived;
        private string? _archiveReason;

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

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Date)));
                }
            }
        }

        public string ShortReason
        {
            get => _shortReason;
            set
            {
                if (_shortReason != value)
                {
                    _shortReason = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShortReason)));
                }
            }
        }

        public string LongReason
        {
            get => _longReason;
            set
            {
                if (_longReason != value)
                {
                    _longReason = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LongReason)));
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

        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
