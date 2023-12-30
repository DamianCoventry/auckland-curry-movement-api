using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class SelectSingleMemberPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<MemberViewModel> _members = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public SelectSingleMemberPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Action<MemberViewModel>? AcceptFunction { get; set; } = null;
        public Action? CancelFunction { get; set; } = null;
        public MemberViewModel? SelectedMember { get; set; } = null;
        private MemberViewModel? OriginalSelection { get; set; } = null;
        public int PageSize { get; set; } = 10;

        public ObservableCollection<MemberViewModel> Members
        {
            get => _members;
            set
            {
                _members = [];
                foreach (var member in value)
                {
                    _members.Add(new MemberViewModel()
                    {
                        ID = member.ID,
                        Name = member.Name,
                        ArchiveReason = member.ArchiveReason,
                        IsArchived = member.IsArchived,
                    });
                }
                OnPropertyChanged(nameof(Members));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (SelectedMember != null)
            {
                OriginalSelection = new MemberViewModel()
                {
                    ID = SelectedMember.ID,
                    Name = SelectedMember.Name,
                    ArchiveReason = SelectedMember.ArchiveReason,
                    IsArchived = SelectedMember.IsArchived,
                };
            }

            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
        }

        private async Task LoadData()
        {
            try
            {
                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                AcceptChanges.IsEnabled = false;
                DiscardChanges.IsEnabled = false;

                await Task.Delay(750);
                var pretendRdbms = new List<MemberViewModel>() {
                    new() { ID = 1, Name = "Alfred Sanchez" },
                    new() { ID = 2, Name = "Kyle Scott" },
                    new() { ID = 3, Name = "Kaiser Farley" },
                    new() { ID = 4, Name = "Reginald Weber" },
                    new() { ID = 5, Name = "Cullen Schmitt" },
                    new() { ID = 6, Name = "Kenneth Allen" },
                    new() { ID = 7, Name = "Jaxtyn Erickson" },
                    new() { ID = 8, Name = "Barrett Herrera" },
                    new() { ID = 9, Name = "Lee Dickerson" },
                    new() { ID = 10, Name = "Edwin Spears" },
                    new() { ID = 11, Name = "Luke Wood" },
                    new() { ID = 12, Name = "Ben Vega" },
                    new() { ID = 13, Name = "Marlowe Ho" },
                    new() { ID = 14, Name = "Jaxton Hopkins" },
                    new() { ID = 15, Name = "Allen Rangel" },
                    new() { ID = 16, Name = "Fisher Willis" },
                    new() { ID = 17, Name = "Terrance Cain" },
                    new() { ID = 18, Name = "Alonso Pratt" },
                    new() { ID = 19, Name = "Sonny Brandt" },
                    new() { ID = 20, Name = "Asher Gates" },
                    new() { ID = 21, Name = "Bruno Holmes" },
                    new() { ID = 22, Name = "Hunter Gonzalez" },
                    new() { ID = 23, Name = "Kendrick Shields" },
                    new() { ID = 24, Name = "Braydon Brooks" },
                    new() { ID = 25, Name = "Thomas Norris" },
                    new() { ID = 26, Name = "Gage Suarez" },
                    new() { ID = 27, Name = "Kolby Stone" },
                    new() { ID = 28, Name = "Eden Berger" },
                    new() { ID = 29, Name = "Memphis Blake" },
                    new() { ID = 30, Name = "Eugene Callahan" },
                    new() { ID = 31, Name = "Landin Mayer" },
                    new() { ID = 32, Name = "Reginald Cervantes" },
                    new() { ID = 33, Name = "Trevon Bird" },
                    new() { ID = 34, Name = "Mauricio Patterson" },
                };

                _totalPages = pretendRdbms.Count / PageSize;
                if (pretendRdbms.Count % PageSize > 0)
                    _totalPages++;
                _currentPage = Math.Min(Math.Max(0, _currentPage), _totalPages - 1);

                LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;

                MergePageIntoListView(pretendRdbms.Skip(_currentPage * PageSize).Take(PageSize).ToList());

                await Task.Delay(750);

                if (OriginalSelection != null)
                {
                    foreach (var member in Members)
                    {
                        if (OriginalSelection != null && OriginalSelection.ID == member.ID && OriginalSelection.Name == member.Name)
                        {
                            OriginalSelection = null;
                            MemberItems.SelectedItem = member;
                        }
                    }
                }

                var elapsed = DateTime.Now - startTime;
                if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                    await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                AcceptChanges.IsEnabled = true;
                DiscardChanges.IsEnabled = true;
            }
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
            CancelFunction?.Invoke();
            await Navigation.PopAsync();
        }

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            if (MemberItems.SelectedItem == null)
            {
                var toast = Toast.Make("Please select a member");
                await toast.Show();
                return;
            }

            if (MemberItems.SelectedItem is MemberViewModel x)
            {
                SelectedMember = x;
                AcceptFunction?.Invoke(SelectedMember);
                await Navigation.PopAsync();
            }
        }

        private void MergePageIntoListView(List<MemberViewModel> pageOfMembers)
        {
            MemberItems.BatchBegin();
            foreach (var member in pageOfMembers)
            {
                var x = _members.FirstOrDefault(y => y.ID == member.ID);
                if (x == null)
                {
                    _members.Add(new MemberViewModel()
                    {
                        ID = member.ID,
                        Name = member.Name,
                        ArchiveReason = member.ArchiveReason,
                        IsArchived = member.IsArchived,
                    });
                }
            }
            MemberItems.BatchCommit();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
            }
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
