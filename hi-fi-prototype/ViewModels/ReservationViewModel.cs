using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        public static ReservationViewModel? FromModel(Reservation? model)
        {
            if (model == null) return null;
            return new ReservationViewModel()
            {
                ID = model.ID,
                OrganiserID = model.OrganiserID,
                Organiser = MemberViewModel.FromModel(model.Organiser),
                RestaurantID = model.RestaurantID,
                Restaurant = RestaurantViewModel.FromModel(model.Restaurant),
                Year = model.Year,
                Month = model.Month,
                ExactDateTime = model.ExactDateTime,
                NegotiatedBeerPrice = model.NegotiatedBeerPrice,
                NegotiatedBeerDiscount = model.NegotiatedBeerDiscount,
                IsAmnesty = model.IsAmnesty,
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
        private int _organiserID;
        private MemberViewModel? _organiser;
        private int _restaurantID;
        private RestaurantViewModel? _restaurant;
        private int _year;
        private int _month;
        private DateTime _exactDateTime;
        private double? _negotiatedBeerPrice;
        private double? _negotiatedBeerDiscount;
        private bool _isAmnesty;

        public int? ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public int OrganiserID
        {
            get { return _organiserID; }
            set { SetProperty(ref _organiserID, value); }
        }

        public MemberViewModel? Organiser
        {
            get { return _organiser; }
            set { SetProperty(ref _organiser, value); }
        }

        public int RestaurantID
        {
            get { return _restaurantID; }
            set { SetProperty(ref _restaurantID, value); }
        }

        public RestaurantViewModel? Restaurant
        {
            get { return _restaurant; }
            set { SetProperty(ref _restaurant, value); }
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

    }
}
