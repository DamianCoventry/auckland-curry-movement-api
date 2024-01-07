using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class AttendeeViewModel : INotifyPropertyChanged
    {
        public static AttendeeViewModel? FromModel(Attendee? model)
        {
            if (model == null) return null;
            return new AttendeeViewModel()
            {
                ID = model.ID,
                DinnerID = model.DinnerID,
                Dinner = DinnerViewModel.FromModel(model.Dinner),
                MemberID = model.MemberID,
                Membership = MembershipViewModel.FromModel(model.Membership),
                LevelID = model.LevelID,
                Level = LevelViewModel.FromModel(model.Level),
                IsSponsor = model.IsSponsor,
                IsInductee = model.IsInductee,
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
        private int _memberID;
        private MembershipViewModel? _membership;
        private int _levelID;
        private LevelViewModel? _level;
        private bool _isSponsor;
        private bool _isInductee;

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

        public int LevelID
        {
            get { return _levelID; }
            set { SetProperty(ref _levelID, value); }
        }

        public LevelViewModel? Level
        {
            get { return _level; }
            set { SetProperty(ref _level, value); }
        }

        public bool IsSponsor
        {
            get { return _isSponsor; }
            set { SetProperty(ref _isSponsor, value); }
        }

        public bool IsInductee
        {
            get { return _isInductee; }
            set { SetProperty(ref _isInductee, value); }
        }

    }
}
