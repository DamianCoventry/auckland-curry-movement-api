using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Membership : INotifyPropertyChanged
    {
        private int _memberID;
        private int _clubID;
        private bool _isFoundingFather;

        public int MemberID
        {
            get => _memberID;
            set
            {
                if (_memberID != value)
                {
                    _memberID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberID)));
                }
            }
        }

        public int ClubID
        {
            get => _clubID;
            set
            {
                if (_clubID != value)
                {
                    _clubID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ClubID)));
                }
            }
        }

        public bool IsFoundingFather
        {
            get => _isFoundingFather;
            set
            {
                if (_isFoundingFather != value)
                {
                    _isFoundingFather = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFoundingFather)));
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
