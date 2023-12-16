using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Reservation : INotifyPropertyChanged
    {
        static public Reservation? FromModel(acm_models.Reservation? model)
        {
            if (model == null) return null;
            return new Reservation()
            {
                _id = model.ID,
                _organiserID = model.OrganiserID,
                _organiser = Member.FromModel(model.Organiser),
                _restaurantID = model.RestaurantID,
                _restaurant = Restaurant.FromModel(model.Restaurant),
                _year = model.Year,
                _month = model.Month,
                _exactDateTime = model.ExactDateTime,
                _negotiatedBeerPrice = model.NegotiatedBeerPrice,
                _negotiatedBeerDiscount = model.NegotiatedBeerDiscount,
                _isAmnesty = model.IsAmnesty,
            };
        }

        private int? _id;
        private int _organiserID;
        private Member? _organiser;
        private int _restaurantID;
        private Restaurant? _restaurant;
        private int _year;
        private int _month;
        private DateTime _exactDateTime;
        private double? _negotiatedBeerPrice;
        private double? _negotiatedBeerDiscount;
        public bool _isAmnesty;

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

        public int OrganiserID
        {
            get => _organiserID;
            set
            {
                if (_organiserID != value)
                {
                    _organiserID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrganiserID)));
                }
            }
        }

        public Member? Organiser
        {
            get => _organiser;
            set
            {
                if (_organiser != value)
                {
                    _organiser = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Organiser)));
                }
            }
        }

        public int RestaurantID
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

        public int Year
        {
            get => _year;
            set
            {
                if (_year != value)
                {
                    _year = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Year)));
                }
            }
        }

        public int Month
        {
            get => _month;
            set
            {
                if (_month != value)
                {
                    _month = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Month)));
                }
            }
        }

        public DateTime ExactDateTime
        {
            get => _exactDateTime;
            set
            {
                if (_exactDateTime != value)
                {
                    _exactDateTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExactDateTime)));
                }
            }
        }

        public double? NegotiatedBeerPrice
        {
            get => _negotiatedBeerPrice;
            set
            {
                if (_negotiatedBeerPrice != value)
                {
                    _negotiatedBeerPrice = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NegotiatedBeerPrice)));
                }
            }
        }

        public double? NegotiatedBeerDiscount
        {
            get => _negotiatedBeerDiscount;
            set
            {
                if (_negotiatedBeerDiscount != value)
                {
                    _negotiatedBeerDiscount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NegotiatedBeerDiscount)));
                }
            }
        }

        public bool IsAmnesty
        {
            get => _isAmnesty;
            set
            {
                if (_isAmnesty != value)
                {
                    _isAmnesty = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAmnesty)));
                }
            }
        }

        public Dinner? Dinner { get; set; }
        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
