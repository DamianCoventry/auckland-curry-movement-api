using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class ExemptionViewModel : INotifyPropertyChanged
    {
        public static ExemptionViewModel? FromModel(Exemption? model)
        {
            if (model == null) return null;
            return new ExemptionViewModel()
            {
                ID = model.ID,
                FoundingFatherID = model.FoundingFatherID,
                FoundingFather = MembershipViewModel.FromModel(model.FoundingFather),
                MemberID = model.MemberID,
                Membership = MembershipViewModel.FromModel(model.Membership),
                Date = model.Date,
                ShortReason = model.ShortReason,
                LongReason = model.LongReason,
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
        private int _foundingFatherID;
        private MembershipViewModel? _foundingFather;
        private int _memberID;
        private MembershipViewModel? _membership;
        private DateTime _date;
        private string _shortReason = string.Empty;
        private string _longReason = string.Empty;
        private bool _isArchived;
        private string? _archiveReason;

        public int? ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
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

        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public string ShortReason
        {
            get { return _shortReason; }
            set { SetProperty(ref _shortReason, value); }
        }

        public string LongReason
        {
            get { return _longReason; }
            set { SetProperty(ref _longReason, value); }
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
