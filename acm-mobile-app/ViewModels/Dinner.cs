using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Dinner : INotifyPropertyChanged
    {
        static public Dinner? FromModel(acm_models.Dinner? model)
        {
            if (model == null) return null;
            return new Dinner()
            {
                _id = model.ID,
                _reservationID = model.ReservationID,
                _reservation = Reservation.FromModel(model.Reservation),
                _costPerPerson = model.CostPerPerson,
                _numBeersConsumed = model.NumBeersConsumed,
                _kotC = KotC.FromModel(model.KotC)
            };
        }

        private int? _id;
        private int _reservationID;
        private Reservation? _reservation;
        private double? _costPerPerson;
        private int? _numBeersConsumed;
        private KotC? _kotC;

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

        public int ReservationID
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

        public bool HaveCostPerPerson { get => _costPerPerson != null; }

        public double? CostPerPerson
        {
            get => _costPerPerson;
            set
            {
                if (_costPerPerson != value)
                {
                    _costPerPerson = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CostPerPerson)));
                }
            }
        }

        public bool HaveNumBeersConsumed { get => _numBeersConsumed != null; }

        public int? NumBeersConsumed
        {
            get => _numBeersConsumed;
            set
            {
                if (_numBeersConsumed != value)
                {
                    _numBeersConsumed = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumBeersConsumed)));
                }
            }
        }

        public bool HaveKotC { get => _kotC != null; }

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

        public ICollection<Attendee>? Attendees { get; set; }
        public ICollection<Member>? Members { get; set; }
        public ICollection<Violation>? Violations { get; set; }
        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
