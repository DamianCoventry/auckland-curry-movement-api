using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class KotC : INotifyPropertyChanged
    {
        static public KotC? FromModel(acm_models.KotC? model)
        {
            if (model == null) return null;
            return new KotC()
            {
                _id = model.ID,
                _memberID = model.MemberID,
                _member = Member.FromModel(model.Member),
                _dinnerID = model.DinnerID,
                _dinner = Dinner.FromModel(model.Dinner),
                _numChillisConsumed = model.NumChillisConsumed,
            };
        }

        private int? _id;
        private int _memberID;
        private Member? _member;
        private int _dinnerID;
        private Dinner? _dinner;
        private int _numChillisConsumed;

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

        public int NumChillisConsumed
        {
            get => _numChillisConsumed;
            set
            {
                if (_numChillisConsumed != value)
                {
                    _numChillisConsumed = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumChillisConsumed)));
                }
            }
        }

        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
