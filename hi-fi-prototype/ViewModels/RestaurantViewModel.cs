using acm_models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hi_fi_prototype.ViewModels
{
    public class RestaurantViewModel : INotifyPropertyChanged
    {
        public static RestaurantViewModel? FromModel(Restaurant? model)
        {
            if (model == null) return null;
            return new RestaurantViewModel()
            {
                ID = model.ID,
                Name = model.Name,
                StreetAddress = model.StreetAddress,
                Suburb = model.Suburb,
                PhoneNumber = model.PhoneNumber,
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
        private string? _streetAddress;
        private string _suburb = string.Empty;
        private string? _phoneNumber;
        private bool _isArchived;
        private string? _archiveReason;
        private bool _isDeleting = false;

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

        public string? StreetAddress
        {
            get { return _streetAddress; }
            set { SetProperty(ref _streetAddress, value); }
        }

        public string Suburb
        {
            get { return _suburb; }
            set { SetProperty(ref _suburb, value); }
        }

        public string? PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
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

        public bool IsDeleting
        {
            get { return _isDeleting; }
            set { SetProperty(ref _isDeleting, value); }
        }
    }
}
