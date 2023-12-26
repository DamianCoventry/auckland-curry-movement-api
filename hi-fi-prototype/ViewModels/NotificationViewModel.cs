using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        static public NotificationViewModel FromModel(Notification model)
        {
            return new NotificationViewModel()
            {
                _id = model.ID,
                _date = model.Date,
                _shortDescription = model.ShortDescription,
                _longDescription = model.LongDescription,
                _attendeeID = model.AttendeeID,
                // TODO: _attendee = Attendee.FromModel(model.Attendee),
                _clubID = model.ClubID,
                // TODO: _club = Club.FromModel(model.Club),
                _dinnerID = model.DinnerID,
                // TODO: _dinner = Dinner.FromModel(model.Dinner),
                _exemptionID = model.ExemptionID,
                // TODO: _exemption = Exemption.FromModel(model.Exemption),
                _kotCID = model.KotCID,
                // TODO: _kotC = KotC.FromModel(model.KotC),
                _levelID = model.LevelID,
                // TODO: _level = Level.FromModel(model.Level),
                _memberID = model.MemberID,
                // TODO: _member = Member.FromModel(model.Member),
                _reservationID = model.ReservationID,
                // TODO: _reservation = Reservation.FromModel(model.Reservation),
                _restaurantID = model.RestaurantID,
                // TODO: _restaurant = Restaurant.FromModel(model.Restaurant),
                _rotYYear = model.RotYYear,
                // TODO: _rotY = RotY.FromModel(model.RotY),
                _violationID = model.ViolationID,
                // TODO: _violation = Violation.FromModel(model.Violation),
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

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int? ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public string ShortDescription
        {
            get { return _shortDescription; }
            set { SetProperty(ref _shortDescription, value); }
        }

        public string LongDescription
        {
            get { return _longDescription; }
            set { SetProperty(ref _longDescription, value); }
        }

        public bool HasAttendee { get { return _attendee != null; } }
        public bool HasClub { get { return _club != null; } }
        public bool HasDinner { get { return _dinner != null; } }
        public bool HasExemption { get { return _exemption != null; } }
        public bool HasKotC { get { return _kotC != null; } }
        public bool HasLevel { get { return _level != null; } }
        public bool HasMember { get { return _member != null; } }
        public bool HasReservation { get { return _reservation != null; } }
        public bool HasRestaurant { get { return _restaurant != null; } }
        public bool HasRotY { get { return _rotY != null; } }
        public bool HasViolation { get { return _violation != null; } }

        public int? AttendeeID
        {
            get { return _attendeeID; }
            set { SetProperty(ref _attendeeID, value); }
        }

        public Attendee? Attendee
        {
            get { return _attendee; }
            set { SetProperty(ref _attendee, value); }
        }

        public int? ClubID
        {
            get { return _clubID; }
            set { SetProperty(ref _clubID, value); }
        }

        public Club? Club
        {
            get { return _club; }
            set { SetProperty(ref _club, value); }
        }

        public int? DinnerID
        {
            get { return _dinnerID; }
            set { SetProperty(ref _dinnerID, value); }
        }

        public Dinner? Dinner
        {
            get { return _dinner; }
            set { SetProperty(ref _dinner, value); }
        }

        public int? ExemptionID
        {
            get { return _exemptionID; }
            set { SetProperty(ref _exemptionID, value); }
        }

        public Exemption? Exemption
        {
            get { return _exemption; }
            set { SetProperty(ref _exemption, value); }
        }

        public int? KotCID
        {
            get { return _kotCID; }
            set { SetProperty(ref _kotCID, value); }
        }

        public KotC? KotC
        {
            get { return _kotC; }
            set { SetProperty(ref _kotC, value); }
        }

        public int? LevelID
        {
            get { return _levelID; }
            set { SetProperty(ref _levelID, value); }
        }

        public Level? Level
        {
            get { return _level; }
            set { SetProperty(ref _level, value); }
        }

        public int? MemberID
        {
            get { return _memberID; }
            set { SetProperty(ref _memberID, value); }
        }

        public Member? Member
        {
            get { return _member; }
            set { SetProperty(ref _member, value); }
        }

        public int? ReservationID
        {
            get { return _reservationID; }
            set { SetProperty(ref _reservationID, value); }
        }

        public Reservation? Reservation
        {
            get { return _reservation; }
            set { SetProperty(ref _reservation, value); }
        }

        public int? RestaurantID
        {
            get { return _restaurantID; }
            set { SetProperty(ref _restaurantID, value); }
        }

        public Restaurant? Restaurant
        {
            get { return _restaurant; }
            set { SetProperty(ref _restaurant, value); }
        }

        public int? RotYYear
        {
            get { return _rotYYear; }
            set { SetProperty(ref _rotYYear, value); }
        }

        public RotY? RotY
        {
            get { return _rotY; }
            set { SetProperty(ref _rotY, value); }
        }

        public int? ViolationID
        {
            get { return _violationID; }
            set { SetProperty(ref _violationID, value); }
        }

        public Violation? Violation
        {
            get { return _violation; }
            set { SetProperty(ref _violation, value); }
        }
    }
}
