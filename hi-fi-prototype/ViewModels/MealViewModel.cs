using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class MealViewModel : INotifyPropertyChanged
    {
        static public MealViewModel? FromModel(Meal? model)
        {
            if (model == null) return null;
            return new MealViewModel()
            {
                ReservationID = model.ReservationID,
                OrganiserID = model.OrganiserID,
                OrganiserName = model.OrganiserName,
                RestaurantID = model.RestaurantID,
                RestaurantName = model.RestaurantName,
                Year = model.Year,
                Month = model.Month,
                ExactDateTime = model.ExactDateTime,
                NegotiatedBeerPrice = model.NegotiatedBeerPrice,
                NegotiatedBeerDiscount = model.NegotiatedBeerDiscount,
                IsAmnesty = model.IsAmnesty,
                DinnerID = model.DinnerID,
                CostPerPerson = model.CostPerPerson,
                NumBeersConsumed = model.NumBeersConsumed,
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

        private int _reservationID;
        private int _organiserID;
        private string _organiserName = string.Empty;
        private int _restaurantID;
        private string _restaurantName = string.Empty;
        private int _year;
        private int _month;
        private DateTime _exactDateTime;
        private double? _negotiatedBeerPrice;
        private double? _negotiatedBeerDiscount;
        private bool _isAmnesty;
        private int? _dinnerID;
        private double? _costPerPerson;
        private int? _numBeersConsumed;

        public int ReservationID
        {
            get { return _reservationID; }
            set { SetProperty(ref _reservationID, value); }
        }

        public int OrganiserID
        {
            get { return _organiserID; }
            set { SetProperty(ref _organiserID, value); }
        }

        public string OrganiserName
        {
            get { return _organiserName; }
            set { SetProperty(ref _organiserName, value); }
        }

        public int RestaurantID
        {
            get { return _restaurantID; }
            set { SetProperty(ref _restaurantID, value); }
        }

        public string RestaurantName
        {
            get { return _restaurantName; }
            set { SetProperty(ref _restaurantName, value); }
        }

        public int Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
        }

        public int Month
        {
            get { return _month; }
            set { SetProperty(ref _month, value); }
        }

        public DateTime ExactDateTime
        {
            get { return _exactDateTime; }
            set { SetProperty(ref _exactDateTime, value); }
        }

        public double? NegotiatedBeerPrice
        {
            get { return _negotiatedBeerPrice; }
            set { SetProperty(ref _negotiatedBeerPrice, value); }
        }

        public double? NegotiatedBeerDiscount
        {
            get { return _negotiatedBeerDiscount; }
            set { SetProperty(ref _negotiatedBeerDiscount, value); }
        }

        public bool IsAmnesty
        {
            get { return _isAmnesty; }
            set { SetProperty(ref _isAmnesty, value); }
        }

        public int? DinnerID
        {
            get { return _dinnerID; }
            set { SetProperty(ref _dinnerID, value); }
        }

        public double? CostPerPerson
        {
            get { return _costPerPerson; }
            set { SetProperty(ref _costPerPerson, value); }
        }

        public int? NumBeersConsumed
        {
            get { return _numBeersConsumed; }
            set { SetProperty(ref _numBeersConsumed, value); }
        }

        public bool HasDinner { get => _dinnerID != null; }
        public bool HasNoDinner { get => _dinnerID == null; }
    }
}
