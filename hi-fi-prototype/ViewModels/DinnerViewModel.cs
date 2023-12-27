using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class DinnerViewModel : INotifyPropertyChanged
    {
        public static DinnerViewModel? FromModel(Dinner? model)
        {
            if (model == null) return null;
            return new DinnerViewModel()
            {
                ID = model.ID,
                ReservationID = model.ReservationID,
                Reservation = ReservationViewModel.FromModel(model.Reservation),
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

        private int? _id;
        private int _reservationID;
        private ReservationViewModel? _reservation;
        private double? _costPerPerson;
        private int? _numBeersConsumed;

        public int? ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public int ReservationID
        {
            get { return _reservationID; }
            set { SetProperty(ref _reservationID, value); }
        }

        public ReservationViewModel? Reservation
        {
            get { return _reservation; }
            set { SetProperty(ref _reservation, value); }
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

    }
}
