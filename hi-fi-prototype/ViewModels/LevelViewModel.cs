using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class LevelViewModel : INotifyPropertyChanged
    {
        public static LevelViewModel? FromModel(Level? model)
        {
            if (model == null) return null;
            return new LevelViewModel()
            {
                ID = model.ID,
                RequiredAttendances = model.RequiredAttendances,
                Name = model.Name,
                Description = model.Description,
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
        private int _requiredAttendances;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private bool _isArchived;
        private string? _archiveReason;

        public int? ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public int RequiredAttendances
        {
            get { return _requiredAttendances; }
            set { SetProperty(ref _requiredAttendances, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
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
