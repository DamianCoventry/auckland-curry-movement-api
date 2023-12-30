using acm_models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class ClubViewModel : INotifyPropertyChanged
    {
        public static ClubViewModel? FromModel(Club? model)
        {
            if (model == null) return null;
            return new ClubViewModel()
            {
                ID = model.ID,
                Name = model.Name,
                NumMembers = model.Members != null ? model.Members.Count : 0,
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
        private bool _isArchived;
        private string? _archiveReason;
        private ObservableCollection<MemberViewModel> _foundingFathers = [];
        private int _numMembers = 0;

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

        public ObservableCollection<MemberViewModel> FoundingFathers
        {
            get { return _foundingFathers; }
            set { SetProperty(ref _foundingFathers, value); }
        }

        public int NumMembers
        {
            get { return _numMembers; }
            set { SetProperty(ref _numMembers, value); }
        }

        public string NumMembersString { get => $"Club has {NumMembers} members."; }
    }
}
