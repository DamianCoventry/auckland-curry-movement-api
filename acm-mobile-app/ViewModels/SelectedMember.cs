using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class SelectedMember : INotifyPropertyChanged
    {
        private bool _selected;
        private Member _member = new();

        public bool IsSelected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public Member Member
        {
            get => _member;
            set
            {
                if (_member.ID != value.ID || _member.Name != value.Name || _member.SponsorID != value.SponsorID ||
                    _member.CurrentLevelID != value.CurrentLevelID || _member.AttendanceCount != value.AttendanceCount)
                {
                    _member = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
