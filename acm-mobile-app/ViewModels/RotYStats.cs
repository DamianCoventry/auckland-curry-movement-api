using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class RotYStats : INotifyPropertyChanged
    {
        static public RotYStats? FromModel(acm_models.RotYStats? model)
        {
            if (model == null) return null;
            return new RotYStats()
            {
                _restaurantID = model.RestaurantID,
                _isCurrentRotY = model.IsCurrentRotY,
                _currentYear = model.CurrentYear,
                _formerYears = model.FormerYears,
            };
        }

        private int _restaurantID;
        private bool _isCurrentRotY;
        private int _currentYear;
        private List<int> _formerYears = [];

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

        public int CurrentYear
        {
            get => _currentYear;
            set
            {
                if (_currentYear != value)
                {
                    _currentYear = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentYear)));
                }
            }
        }

        public List<int> FormerYears
        {
            get => _formerYears;
            set
            {
                if (_formerYears != value)
                {
                    _formerYears = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FormerYears)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
