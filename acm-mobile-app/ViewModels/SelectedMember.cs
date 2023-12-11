using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class SelectedMember : INotifyPropertyChanged
    {
        private bool _selected;
        private bool _isFoundingFather;
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

        public bool IsFoundingFather
        {
            get => _isFoundingFather;
            set
            {
                if (_isFoundingFather != value)
                {
                    _isFoundingFather = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFoundingFather)));
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
