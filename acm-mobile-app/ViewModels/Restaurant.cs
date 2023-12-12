using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace acm_mobile_app.ViewModels
{
    public class Restaurant : INotifyPropertyChanged
    {
        static public Restaurant? FromModel(acm_models.Restaurant? model)
        {
            if (model == null) return null;
            return new Restaurant()
            {
                _id = model.ID,
                _name = model.Name,
                _streetAddress = model.StreetAddress,
                _suburb = model.Suburb,
                _phoneNumber = model.PhoneNumber,
                _isArchived = model.IsArchived,
                _archiveReason = model.ArchiveReason,
            };
        }

        private int? _id;
        private string _name = string.Empty;
        private string? _streetAddress;
        private string _suburb = string.Empty;
        private string? _phoneNumber;
        private bool _isArchived;
        private string? _archiveReason;

        public int? ID
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        [Column("Street Address")]
        public string? StreetAddress
        {
            get => _streetAddress;
            set
            {
                if (_streetAddress != value)
                {
                    _streetAddress = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StreetAddress)));
                }
            }
        }

        public string Suburb
        {
            get => _suburb;
            set
            {
                if (_suburb != value)
                {
                    _suburb = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suburb)));
                }
            }
        }

        [Column("Phone Number")]
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PhoneNumber)));
                }
            }
        }

        public bool IsArchived
        {
            get => _isArchived;
            set
            {
                if (_isArchived != value)
                {
                    _isArchived = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsArchived)));
                }
            }
        }

        public string? ArchiveReason
        {
            get => _archiveReason;
            set
            {
                if (_archiveReason != value)
                {
                    _archiveReason = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArchiveReason)));
                }
            }
        }

        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<RotY>? RotYs { get; set; }
        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
