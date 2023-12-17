using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class RotY : INotifyPropertyChanged
    {
        static public RotY? FromModel(acm_models.RotY? model)
        {
            if (model == null) return null;
            return new RotY()
            {
                _year = model.Year,
                _restaurantID = model.RestaurantID,
                _restaurant = Restaurant.FromModel(model.Restaurant),
                _numVotes = model.NumVotes,
                _totalVotes = model.TotalVotes,
                _winningScore = model.WinningScore,
                _presenterID = model.PresenterID,
                _presenter = Member.FromModel(model.Presenter)
            };
        }

        private int? _year;
        private int _restaurantID;
        private Restaurant? _restaurant;
        private int _numVotes;
        private int _totalVotes;
        private double _winningScore;
        private int? _presenterID;
        private Member? _presenter;

        public int? Year
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

        public int NumVotes
        {
            get => _numVotes;
            set
            {
                if (_numVotes != value)
                {
                    _numVotes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumVotes)));
                }
            }
        }

        public int TotalVotes
        {
            get => _totalVotes;
            set
            {
                if (_totalVotes != value)
                {
                    _totalVotes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalVotes)));
                }
            }
        }

        public double WinningScore
        {
            get => _winningScore;
            set
            {
                if (_winningScore != value)
                {
                    _winningScore = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WinningScore)));
                }
            }
        }

        public int? PresenterID
        {
            get => _presenterID;
            set
            {
                if (_presenterID != value)
                {
                    _presenterID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PresenterID)));
                }
            }
        }

        public Member? Presenter
        {
            get => _presenter;
            set
            {
                if (_presenter != value)
                {
                    _presenter = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Presenter)));
                }
            }
        }

        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
