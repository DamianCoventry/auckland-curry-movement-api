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
                Member = MemberViewModel.FromModel(model.Member),
                ClubID = model.ClubID,
                SponsorID = model.SponsorID,
                Sponsor = FromModel(model.Sponsor), // TODO: check for and disallow recursion
                LevelID = model.LevelID,
                Level = LevelViewModel.FromModel(model.Level),
                AttendanceCount = model.AttendanceCount,
                IsAdmin = model.IsAdmin,
                IsFoundingFather = model.IsFoundingFather,
                IsAuditor = model.IsAuditor,
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
        private MemberViewModel? _membership;
        private int _clubID;
        private int? _sponsorID;
        private MembershipViewModel? _sponsor;
        private int _levelID;
        private LevelViewModel? _level;
        private int _attendanceCount;
        private bool _isAdmin;
        private bool _isFoundingFather;
        private bool _isAuditor;

        public int MemberID
        {
            get { return _memberID; }
            set { SetProperty(ref _memberID, value); }
        }

        public MemberViewModel? Member
        {
            get { return _membership; }
            set { SetProperty(ref _membership, value); }
        }

        public int ClubID
        {
            get { return _clubID; }
            set { SetProperty(ref _clubID, value); }
        }

        public int? SponsorID
        {
            get { return _sponsorID; }
            set { SetProperty(ref _sponsorID, value); }
        }

        public MembershipViewModel? Sponsor
        {
            get { return _sponsor; }
            set { SetProperty(ref _sponsor, value); }
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

        public int AttendanceCount
        {
            get { return _attendanceCount; }
            set { SetProperty(ref _attendanceCount, value); }
        }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { SetProperty(ref _isAdmin, value); }
        }

        public bool IsFoundingFather
        {
            get { return _isFoundingFather; }
            set { SetProperty(ref _isFoundingFather, value); }
        }

        public bool IsAuditor
        {
            get { return _isAuditor; }
            set { SetProperty(ref _isAuditor, value); }
        }

    }
}
