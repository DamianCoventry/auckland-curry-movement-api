using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class SelectedMemberViewModel
    {
        public static SelectedMemberViewModel FromModel(Member model, bool isSelected = false)
        {
            return new SelectedMemberViewModel()
            {
                IsSelected = isSelected,
                ID = model.ID,
                Name = model.Name,
                IsArchived = model.IsArchived,
                ArchiveReason = model.ArchiveReason,
            };
        }

        private bool _selected = false;
        private int? _id = null;
        private string _name = string.Empty;
        private bool _isArchived;
        private string? _archiveReason = null;

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

        public bool IsSelected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

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
    }
}
