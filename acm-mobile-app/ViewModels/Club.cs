using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class Club : INotifyPropertyChanged
    {
        static public Club? FromModel(acm_models.Club? model)
        {
            if (model == null) return null;

            var club = new Club
            {
                _id = model.ID,
                _name = model.Name,
                _isArchived = model.IsArchived,
                _archiveReason = model.ArchiveReason,
                Members = []
            };

            if (model.Members != null)
            {
                foreach (var m in model.Members)
                {
                    var x = Member.FromModel(m);
                    if (x != null)
                        club.Members.Add(x);
                }
            }

            return club;
        }

        private int? _id;
        private string _name = string.Empty;
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

        public int NumMembers { get { return Members == null ? 0 : Members.Count; } }

        public virtual ICollection<Member>? Members { get; set; }
        public ICollection<Notification>? Notifications { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
