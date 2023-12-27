using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        public static NotificationViewModel? FromModel(Notification? model)
        {
            if (model == null) return null;
            return new NotificationViewModel()
            {
                ID = model.ID,
                Date = model.Date,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
                AttendeeID = model.AttendeeID,
                Attendee = AttendeeViewModel.FromModel(model.Attendee),
                ClubID = model.ClubID,
                Club = ClubViewModel.FromModel(model.Club),
                DinnerID = model.DinnerID,
                Dinner = DinnerViewModel.FromModel(model.Dinner),
                ExemptionID = model.ExemptionID,
                Exemption = ExemptionViewModel.FromModel(model.Exemption),
                KotCID = model.KotCID,
                KotC = KotCViewModel.FromModel(model.KotC),
                LevelID = model.LevelID,
                Level = LevelViewModel.FromModel(model.Level),
                MemberID = model.MemberID,
                Member = MemberViewModel.FromModel(model.Member),
                ReservationID = model.ReservationID,
                Reservation = ReservationViewModel.FromModel(model.Reservation),
                RestaurantID = model.RestaurantID,
                Restaurant = RestaurantViewModel.FromModel(model.Restaurant),
                RotYYear = model.RotYYear,
                RotY = RotYViewModel.FromModel(model.RotY),
                ViolationID = model.ViolationID,
                Violation = ViolationViewModel.FromModel(model.Violation),
            };
        }

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

        private int? _id;
        private DateTime _date;
        private string _shortDescription = string.Empty;
        private string _longDescription = string.Empty;
        private int? _attendeeID;
        private AttendeeViewModel? _attendee;
        private int? _clubID;
        private ClubViewModel? _club;
        private int? _dinnerID;
        private DinnerViewModel? _dinner;
        private int? _exemptionID;
        private ExemptionViewModel? _exemption;
        private int? _kotCID;
        private KotCViewModel? _kotC;
        private int? _levelID;
        private LevelViewModel? _level;
        private int? _memberID;
        private MemberViewModel? _member;
        private int? _reservationID;
        private ReservationViewModel? _reservation;
        private int? _restaurantID;
        private RestaurantViewModel? _restaurant;
        private int? _rotYYear;
        private RotYViewModel? _rotY;
        private int? _violationID;
        private ViolationViewModel? _violation;

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

        public int? AttendeeID
        {
            get { return _attendeeID; }
            set { SetProperty(ref _attendeeID, value); }
        }

        public bool HasAttendee { get => Attendee != null; }

        public AttendeeViewModel? Attendee
        {
            get { return _attendee; }
            set { SetProperty(ref _attendee, value); }
        }

        public int? ClubID
        {
            get { return _clubID; }
            set { SetProperty(ref _clubID, value); }
        }

        public bool HasClub { get => Club != null; }

        public ClubViewModel? Club
        {
            get { return _club; }
            set { SetProperty(ref _club, value); }
        }

        public int? DinnerID
        {
            get { return _dinnerID; }
            set { SetProperty(ref _dinnerID, value); }
        }

        public bool HasDinner { get => Dinner != null; }

        public DinnerViewModel? Dinner
        {
            get { return _dinner; }
            set { SetProperty(ref _dinner, value); }
        }

        public int? ExemptionID
        {
            get { return _exemptionID; }
            set { SetProperty(ref _exemptionID, value); }
        }

        public bool HasExemption { get => Exemption != null; }

        public ExemptionViewModel? Exemption
        {
            get { return _exemption; }
            set { SetProperty(ref _exemption, value); }
        }

        public int? KotCID
        {
            get { return _kotCID; }
            set { SetProperty(ref _kotCID, value); }
        }

        public bool HasKotC { get => KotC != null; }

        public KotCViewModel? KotC
        {
            get { return _kotC; }
            set { SetProperty(ref _kotC, value); }
        }

        public int? LevelID
        {
            get { return _levelID; }
            set { SetProperty(ref _levelID, value); }
        }

        public bool HasLevel { get => Level != null; }

        public LevelViewModel? Level
        {
            get { return _level; }
            set { SetProperty(ref _level, value); }
        }

        public int? MemberID
        {
            get { return _memberID; }
            set { SetProperty(ref _memberID, value); }
        }

        public bool HasMember { get => Member != null; }

        public MemberViewModel? Member
        {
            get { return _member; }
            set { SetProperty(ref _member, value); }
        }

        public int? ReservationID
        {
            get { return _reservationID; }
            set { SetProperty(ref _reservationID, value); }
        }

        public bool HasReservation { get => Reservation != null; }

        public ReservationViewModel? Reservation
        {
            get { return _reservation; }
            set { SetProperty(ref _reservation, value); }
        }

        public int? RestaurantID
        {
            get { return _restaurantID; }
            set { SetProperty(ref _restaurantID, value); }
        }

        public bool HasRestaurant { get => Restaurant != null; }

        public RestaurantViewModel? Restaurant
        {
            get { return _restaurant; }
            set { SetProperty(ref _restaurant, value); }
        }

        public int? RotYYear
        {
            get { return _rotYYear; }
            set { SetProperty(ref _rotYYear, value); }
        }

        public bool HasRotY { get => RotY != null; }

        public RotYViewModel? RotY
        {
            get { return _rotY; }
            set { SetProperty(ref _rotY, value); }
        }

        public int? ViolationID
        {
            get { return _violationID; }
            set { SetProperty(ref _violationID, value); }
        }

        public bool HasViolation { get => Violation != null; }

        public ViolationViewModel? Violation
        {
            get { return _violation; }
            set { SetProperty(ref _violation, value); }
        }

    }
}
