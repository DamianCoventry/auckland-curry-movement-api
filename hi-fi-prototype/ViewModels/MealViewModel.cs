using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class MealViewModel : INotifyPropertyChanged
    {
        public static MealViewModel? FromModel(Reservation? reservation = null, Dinner? dinner = null)
        {
            if (reservation == null && dinner == null) return null;
            var mealViewModel = new MealViewModel();

            if (reservation != null)
            {
                mealViewModel.Reservation = new ReservationViewModel()
                {
                    ID = reservation.ID,
                    OrganiserID = reservation.OrganiserID,
                    Organiser = MemberViewModel.FromModel(reservation.Organiser),
                    RestaurantID = reservation.RestaurantID,
                    Restaurant = RestaurantViewModel.FromModel(reservation.Restaurant),
                    Year = reservation.Year,
                    Month = reservation.Month,
                    ExactDateTime = reservation.ExactDateTime,
                    NegotiatedBeerPrice = reservation.NegotiatedBeerPrice,
                    NegotiatedBeerDiscount = reservation.NegotiatedBeerDiscount,
                    IsAmnesty = reservation.IsAmnesty,
                };
            }

            if (dinner != null)
            {
                mealViewModel.Dinner = new DinnerViewModel()
                {
                    ID = dinner.ID,
                    ReservationID = dinner.ReservationID,
                    Reservation = ReservationViewModel.FromModel(dinner.Reservation),
                    CostPerPerson = dinner.CostPerPerson,
                    NumBeersConsumed = dinner.NumBeersConsumed,
                };
            }

            return mealViewModel;
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

        private ReservationViewModel? _reservation;
        private DinnerViewModel? _dinner;

        public bool HasReservation { get { return _reservation != null; } }

        public ReservationViewModel? Reservation
        {
            get { return _reservation; }
            set { SetProperty(ref _reservation, value); }
        }

        public bool HasDinner { get { return _dinner != null; } }

        public DinnerViewModel? Dinner
        {
            get { return _dinner; }
            set { SetProperty(ref _dinner, value); }
        }
    }
}
