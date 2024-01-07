using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class RotYViewModel : INotifyPropertyChanged
    {
        public static RotYViewModel? FromModel(RotY? model)
        {
            if (model == null) return null;
            return new RotYViewModel()
            {
                Year = model.Year,
                RestaurantID = model.RestaurantID,
                Restaurant = RestaurantViewModel.FromModel(model.Restaurant),
                NumVotes = model.NumVotes,
                WinningScore = model.WinningScore,
                PresenterID = model.PresenterID,
                Presenter = MembershipViewModel.FromModel(model.Presenter),
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

        private int? _year;
        private int _restaurantID;
        private RestaurantViewModel? _restaurant;
        private int _numVotes;
        private int _totalVotes;
        private double _winningScore;
        private int? _presenterID;
        private MembershipViewModel? _presenter;

        public int? Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
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

        public int NumVotes
        {
            get { return _numVotes; }
            set { SetProperty(ref _numVotes, value); }
        }

        public int TotalVotes
        {
            get { return _totalVotes; }
            set { SetProperty(ref _totalVotes, value); }
        }

        public double WinningScore
        {
            get { return _winningScore; }
            set { SetProperty(ref _winningScore, value); }
        }

        public int? PresenterID
        {
            get { return _presenterID; }
            set { SetProperty(ref _presenterID, value); }
        }

        public MembershipViewModel? Presenter
        {
            get { return _presenter; }
            set { SetProperty(ref _presenter, value); }
        }

    }
}
