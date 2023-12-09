using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Level : INotifyPropertyChanged
    {
        static public Level? FromModel(acm_models.Level? model)
        {
            if (model == null) return null;
            return new Level()
            {
                ID = model.ID,
                RequiredAttendances = model.RequiredAttendances,
                Name = model.Name,
                Description = model.Description,
                IsArchived = model.IsArchived,
                ArchiveReason = model.ArchiveReason,
            };
        }

        private int? _id;
        private int _requiredAttendances;
        private string _name = string.Empty;
        private string _description = string.Empty;
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

        public int RequiredAttendances
        {
            get => _requiredAttendances;
            set
            {
                if (_requiredAttendances != value)
                {
                    _requiredAttendances = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RequiredAttendances)));
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

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
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

        public virtual ICollection<Attendee>? Attendees { get; set; }
        public virtual ICollection<Member>? Members { get; set; }
        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
