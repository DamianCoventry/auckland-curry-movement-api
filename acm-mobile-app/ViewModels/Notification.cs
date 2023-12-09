using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Notification : INotifyPropertyChanged
    {
        static public Notification? FromModel(acm_models.Notification? model)
        {
            if (model == null) return null;
            return new Notification()
            {
                _id = model.ID,
                _date = model.Date,
                _shortDescription = model.ShortDescription,
                _longDescription = model.LongDescription,
                _attendeeID = model.AttendeeID,
                _attendee = Attendee.FromModel(model.Attendee),
                _clubID = model.ClubID,
                _club = Club.FromModel(model.Club),
                _dinnerID = model.DinnerID,
                _dinner = Dinner.FromModel(model.Dinner),
                _exemptionID = model.ExemptionID,
                _exemption = Exemption.FromModel(model.Exemption),
                _kotCID = model.KotCID,
                _kotC = KotC.FromModel(model.KotC),
                _levelID = model.LevelID,
                _level = Level.FromModel(model.Level),
                _memberID = model.MemberID,
                _member = Member.FromModel(model.Member),
                _reservationID = model.ReservationID,
                _reservation = Reservation.FromModel(model.Reservation),
                _restaurantID = model.RestaurantID,
                _restaurant = Restaurant.FromModel(model.Restaurant),
                _rotYYear = model.RotYYear,
                _rotY = RotY.FromModel(model.RotY),
                _violationID = model.ViolationID,
                _violation = Violation.FromModel(model.Violation),
            };
        }

        private int? _id;
        private DateTime _date;
        private string _shortDescription = string.Empty;
        private string _longDescription = string.Empty;
        private int? _attendeeID;
        private Attendee? _attendee;
        private int? _clubID;
        private Club? _club;
        private int? _dinnerID;
        private Dinner? _dinner;
        private int? _exemptionID;
        private Exemption? _exemption;
        private int? _kotCID;
        private KotC? _kotC;
        private int? _levelID;
        private Level? _level;
        private int? _memberID;
        private Member? _member;
        private int? _reservationID;
        private Reservation? _reservation;
        private int? _restaurantID;
        private Restaurant? _restaurant;
        private int? _rotYYear;
        private RotY? _rotY;
        private int? _violationID;
        private Violation? _violation;

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

        public string ShortDescription
        {
            get => _shortDescription;
            set
            {
                if (_shortDescription != value)
                {
                    _shortDescription = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShortDescription)));
                }
            }
        }

        public string LongDescription
        {
            get => _longDescription;
            set
            {
                if (_longDescription != value)
                {
                    _longDescription = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LongDescription)));
                }
            }
        }

        public int? AttendeeID
        {
            get => _attendeeID;
            set
            {
                if (_attendeeID != value)
                {
                    _attendeeID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AttendeeID)));
                }
            }
        }

        public Attendee? Attendee
        {
            get => _attendee;
            set
            {
                if (_attendee != value)
                {
                    _attendee = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Attendee)));
                }
            }
        }

        public int? ClubID
        {
            get => _clubID;
            set
            {
                if (_clubID != value)
                {
                    _clubID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ClubID)));
                }
            }
        }

        public Club? Club
        {
            get => _club;
            set
            {
                if (_club != value)
                {
                    _club = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Club)));
                }
            }
        }

        public int? DinnerID
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

        public int? ExemptionID
        {
            get => _exemptionID;
            set
            {
                if (_exemptionID != value)
                {
                    _exemptionID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExemptionID)));
                }
            }
        }

        public Exemption? Exemption
        {
            get => _exemption;
            set
            {
                if (_exemption != value)
                {
                    _exemption = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exemption)));
                }
            }
        }

        public int? KotCID
        {
            get => _kotCID;
            set
            {
                if (_kotCID != value)
                {
                    _kotCID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KotCID)));
                }
            }
        }

        public KotC? KotC
        {
            get => _kotC;
            set
            {
                if (_kotC != value)
                {
                    _kotC = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KotC)));
                }
            }
        }

        public int? LevelID
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

        public int? MemberID
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

        public int? ReservationID
        {
            get => _reservationID;
            set
            {
                if (_reservationID != value)
                {
                    _reservationID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReservationID)));
                }
            }
        }

        public Reservation? Reservation
        {
            get => _reservation;
            set
            {
                if (_reservation != value)
                {
                    _reservation = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Reservation)));
                }
            }
        }

        public int? RestaurantID
        {
            get => _restaurantID;
            set
            {
                if (_restaurantID != value)
                {
                    _restaurantID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RestaurantID)));
                }
            }
        }

        public Restaurant? Restaurant
        {
            get => _restaurant;
            set
            {
                if (_restaurant != value)
                {
                    _restaurant = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Restaurant)));
                }
            }
        }

        public int? RotYYear
        {
            get => _rotYYear;
            set
            {
                if (_rotYYear != value)
                {
                    _rotYYear = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RotYYear)));
                }
            }
        }

        public RotY? RotY
        {
            get => _rotY;
            set
            {
                if (_rotY != value)
                {
                    _rotY = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RotY)));
                }
            }
        }

        public int? ViolationID
        {
            get => _violationID;
            set
            {
                if (_violationID != value)
                {
                    _violationID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViolationID)));
                }
            }
        }

        public Violation? Violation
        {
            get => _violation;
            set
            {
                if (_violation != value)
                {
                    _violation = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Violation)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
