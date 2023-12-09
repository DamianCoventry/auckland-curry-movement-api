using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class PastDinner : INotifyPropertyChanged
    {
        static public PastDinner? FromModel(acm_models.PastDinner? model)
        {
            if (model == null) return null;
            return new PastDinner()
            {
                _id = model.ID,
                _organiserID = model.OrganiserID,
                _organiserName = model.OrganiserName,
                _restaurantID = model.RestaurantID,
                _restaurantName = model.RestaurantName,
                _exactDateTime = model.ExactDateTime,
                _negotiatedBeerPrice = model.NegotiatedBeerPrice,
                _negotiatedBeerDiscount = model.NegotiatedBeerDiscount,
                _costPerPerson = model.CostPerPerson,
                _numBeersConsumed = model.NumBeersConsumed,
                _isNewKotC = model.IsNewKotC,
                _isFormerRotY = model.IsFormerRotY,
                _isCurrentRotY = model.IsCurrentRotY,
                _isRulesViolation = model.IsRulesViolation,
            };
        }

        private int? _id;
        private int _organiserID;
        private string _organiserName = string.Empty;
        private int _restaurantID;
        private string _restaurantName = string.Empty;
        private DateTime _exactDateTime;
        private double? _negotiatedBeerPrice;
        private double? _negotiatedBeerDiscount;
        private double? _costPerPerson;
        private int? _numBeersConsumed;
        private int _isNewKotC;
        private int _isFormerRotY;
        private int _isCurrentRotY;
        private int _isRulesViolation;

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

        public string OrganiserName
        {
            get => _organiserName;
            set
            {
                if (_organiserName != value)
                {
                    _organiserName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrganiserName)));
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

        public string RestaurantName
        {
            get => _restaurantName;
            set
            {
                if (_restaurantName != value)
                {
                    _restaurantName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RestaurantName)));
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

        public int IsNewKotC
        {
            get => _isNewKotC;
            set
            {
                if (_isNewKotC != value)
                {
                    _isNewKotC = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNewKotC)));
                }
            }
        }

        public int IsFormerRotY
        {
            get => _isFormerRotY;
            set
            {
                if (_isFormerRotY != value)
                {
                    _isFormerRotY = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFormerRotY)));
                }
            }
        }

        public int IsCurrentRotY
        {
            get => _isCurrentRotY;
            set
            {
                if (_isCurrentRotY != value)
                {
                    _isCurrentRotY = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCurrentRotY)));
                }
            }
        }

        public int IsRulesViolation
        {
            get => _isRulesViolation;
            set
            {
                if (_isRulesViolation != value)
                {
                    _isRulesViolation = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRulesViolation)));
                }
            }
        }

        public bool NewKotC { get { return IsNewKotC != 0; } }
        public bool FormerRotY { get { return IsFormerRotY != 0; } }
        public bool CurrentRotY { get { return IsCurrentRotY != 0; } }
        public bool RulesViolation { get { return IsRulesViolation != 0; } }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
