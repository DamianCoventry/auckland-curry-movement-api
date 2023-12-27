using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class MembershipViewModel : INotifyPropertyChanged
    {
        public static MembershipViewModel? FromModel(Membership? model)
        {
            if (model == null) return null;
            return new MembershipViewModel()
            {
                MemberID = model.MemberID,
                ClubID = model.ClubID,
                IsFoundingFather = model.IsFoundingFather,
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

        private int _memberID;
        private int _clubID;
        private bool _isFoundingFather;

        public int MemberID
        {
            get { return _memberID; }
            set { SetProperty(ref _memberID, value); }
        }

        public int ClubID
        {
            get { return _clubID; }
            set { SetProperty(ref _clubID, value); }
        }

        public bool IsFoundingFather
        {
            get { return _isFoundingFather; }
            set { SetProperty(ref _isFoundingFather, value); }
        }

    }
}
