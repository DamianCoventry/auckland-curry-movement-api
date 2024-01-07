using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class ViolationViewModel : INotifyPropertyChanged
    {
        public static ViolationViewModel? FromModel(Violation? model)
        {
            if (model == null) return null;
            return new ViolationViewModel()
            {
                ID = model.ID,
                DinnerID = model.DinnerID,
                Dinner = DinnerViewModel.FromModel(model.Dinner),
                FoundingFatherID = model.FoundingFatherID,
                FoundingFather = MembershipViewModel.FromModel(model.FoundingFather),
                MemberID = model.MemberID,
                Membership = MembershipViewModel.FromModel(model.Membership),
                Description = model.Description,
                IsIndianHotCurry = model.IsIndianHotCurry,
                IsReinduction = model.IsReinduction,
                OtherPunishment = model.OtherPunishment,
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
        private int _dinnerID;
        private DinnerViewModel? _dinner;
        private int _foundingFatherID;
        private MembershipViewModel? _foundingFather;
        private int _memberID;
        private MembershipViewModel? _membership;
        private string _description = string.Empty;
        private bool _isIndianHotCurry;
        private bool _isReinduction;
        private string? _otherPunishment;

        public int? ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public int DinnerID
        {
            get { return _dinnerID; }
            set { SetProperty(ref _dinnerID, value); }
        }

        public DinnerViewModel? Dinner
        {
            get { return _dinner; }
            set { SetProperty(ref _dinner, value); }
        }

        public int FoundingFatherID
        {
            get { return _foundingFatherID; }
            set { SetProperty(ref _foundingFatherID, value); }
        }

        public MembershipViewModel? FoundingFather
        {
            get { return _foundingFather; }
            set { SetProperty(ref _foundingFather, value); }
        }

        public int MemberID
        {
            get { return _memberID; }
            set { SetProperty(ref _memberID, value); }
        }

        public MembershipViewModel? Membership
        {
            get { return _membership; }
            set { SetProperty(ref _membership, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public bool IsIndianHotCurry
        {
            get { return _isIndianHotCurry; }
            set { SetProperty(ref _isIndianHotCurry, value); }
        }

        public bool IsReinduction
        {
            get { return _isReinduction; }
            set { SetProperty(ref _isReinduction, value); }
        }

        public string? OtherPunishment
        {
            get { return _otherPunishment; }
            set { SetProperty(ref _otherPunishment, value); }
        }

    }
}
