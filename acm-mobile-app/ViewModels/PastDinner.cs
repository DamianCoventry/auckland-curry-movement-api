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
                _organiserLevelID = model.OrganiserLevelID,
                _isOrganiserFoundingFather = model.IsOrganiserFoundingFather,
                _restaurantID = model.RestaurantID,
                _restaurantName = model.RestaurantName,
                _exactDateTime = model.ExactDateTime,
                _negotiatedBeerPrice = model.NegotiatedBeerPrice,
                _negotiatedBeerDiscount = model.NegotiatedBeerDiscount,
                _costPerPerson = model.CostPerPerson,
                _numBeersConsumed = model.NumBeersConsumed,
                _isNewKotC = model.IsNewKotC != 0,
                _isFormerRotY = model.IsFormerRotY != 0,
                _isCurrentRotY = model.IsCurrentRotY != 0,
                _isRulesViolation = model.IsRulesViolation != 0,
                _isAmnesty = model.IsAmnesty,
            };
        }

        private int? _id;
        private int _organiserID;
        private string _organiserName = string.Empty;
        private int _organiserLevelID;
        private bool _isOrganiserFoundingFather;
        private int _restaurantID;
        private string _restaurantName = string.Empty;
        private DateTime _exactDateTime;
        private double? _negotiatedBeerPrice;
        private double? _negotiatedBeerDiscount;
        private double? _costPerPerson;
        private int? _numBeersConsumed;
        private bool _isNewKotC;
        private bool _isFormerRotY;
        private bool _isCurrentRotY;
        private bool _isRulesViolation;
        private bool _isAmnesty;

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

        public int OrganiserLevelID
        {
            get => _organiserLevelID;
            set
            {
                if (_organiserLevelID != value)
                {
                    _organiserLevelID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrganiserLevelID)));
                }
            }
        }

        // TODO: How to make this programatic?
        public bool IsOrganiserAuditor { get => OrganiserName == "Damian Coventry" && OrganiserID == 6; }

        // TODO: How to make this programatic?
        public bool IsOrganiserGuru { get => OrganiserLevelID == 2; }
        public bool IsOrganiserMaharaja { get => OrganiserLevelID == 3; }

        public bool IsOrganiserFoundingFather
        {
            get => _isOrganiserFoundingFather;
            set
            {
                if (_isOrganiserFoundingFather != value)
                {
                    _isOrganiserFoundingFather = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOrganiserFoundingFather)));
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

        public bool IsNewKotC
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

        public bool IsFormerRotY
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

        public bool IsCurrentRotY
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

        public bool IsRulesViolation
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

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
