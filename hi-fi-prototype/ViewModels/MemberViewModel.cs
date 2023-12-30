using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class MemberViewModel : INotifyPropertyChanged
    {
        public static MemberViewModel? FromModel(Member? model)
        {
            if (model == null) return null;
            return new MemberViewModel()
            {
                ID = model.ID,
                Name = new(model.Name),
                SponsorID = model.SponsorID,
                Sponsor = FromModel(model.Sponsor), // TODO: check for recursion
                CurrentLevelID = model.CurrentLevelID,
                CurrentLevel = LevelViewModel.FromModel(model.CurrentLevel),
                AttendanceCount = model.AttendanceCount,
                IsArchived = model.IsArchived,
                ArchiveReason = model.ArchiveReason,
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
        private string _name = string.Empty;
        private int? _sponsorID;
        private MemberViewModel? _sponsor;
        private int _currentLevelID;
        private LevelViewModel? _currentLevel;
        private int _attendanceCount;
        private bool _isArchived;
        private string? _archiveReason;

        public int? ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public int? SponsorID
        {
            get { return _sponsorID; }
            set { SetProperty(ref _sponsorID, value); }
        }

        public MemberViewModel? Sponsor
        {
            get { return _sponsor; }
            set { SetProperty(ref _sponsor, value); }
        }

        public int CurrentLevelID
        {
            get { return _currentLevelID; }
            set { SetProperty(ref _currentLevelID, value); }
        }

        public LevelViewModel? CurrentLevel
        {
            get { return _currentLevel; }
            set { SetProperty(ref _currentLevel, value); }
        }

        public int AttendanceCount
        {
            get { return _attendanceCount; }
            set { SetProperty(ref _attendanceCount, value); }
        }

        public bool IsArchived
        {
            get { return _isArchived; }
            set { SetProperty(ref _isArchived, value); }
        }

        public string? ArchiveReason
        {
            get { return _archiveReason; }
            set { SetProperty(ref _archiveReason, value); }
        }

    }
}
