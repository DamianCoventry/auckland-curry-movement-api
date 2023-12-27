using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class KotCViewModel : INotifyPropertyChanged
    {
        public static KotCViewModel? FromModel(KotC? model)
        {
            if (model == null) return null;
            return new KotCViewModel()
            {
                ID = model.ID,
                MemberID = model.MemberID,
                Member = MemberViewModel.FromModel(model.Member),
                DinnerID = model.DinnerID,
                Dinner = DinnerViewModel.FromModel(model.Dinner),
                NumChillisConsumed = model.NumChillisConsumed,
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
        private int _memberID;
        private MemberViewModel? _member;
        private int _dinnerID;
        private DinnerViewModel? _dinner;
        private int _numChillisConsumed;

        public int? ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public int MemberID
        {
            get { return _memberID; }
            set { SetProperty(ref _memberID, value); }
        }

        public MemberViewModel? Member
        {
            get { return _member; }
            set { SetProperty(ref _member, value); }
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

        public int NumChillisConsumed
        {
            get { return _numChillisConsumed; }
            set { SetProperty(ref _numChillisConsumed, value); }
        }

    }
}
