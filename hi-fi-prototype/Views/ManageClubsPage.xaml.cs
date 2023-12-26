using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageClubsPage : ContentPage
    {
        private ObservableCollection<ClubViewModel> _clubs = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageClubsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int PageSize { get; set; } = 10;

        public ObservableCollection<ClubViewModel> Clubs
        {
            get => _clubs;
            set
            {
                _clubs = [];
                foreach (var club in value)
                {
                    var copy = new ClubViewModel
                    {
                        ID = club.ID,
                        Name = club.Name,
                        ArchiveReason = club.ArchiveReason,
                        IsArchived = club.IsArchived,
                        FoundingFathers = []
                    };

                    foreach (var ff in club.FoundingFathers)
                    {
                        copy.FoundingFathers.Add(new MemberViewModel()
                        {
                            ID = ff.ID,
                            Name = ff.Name,
                            ArchiveReason = ff.ArchiveReason,
                            IsArchived = ff.IsArchived,
                        });
                    }

                    _clubs.Add(copy);
                }
                OnPropertyChanged(nameof(Clubs));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
        }

        private async void ClubItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is ClubViewModel clubViewModel)
            {
                Dictionary<string, object> parameters = new() { { "Club", clubViewModel } };
                await Shell.Current.GoToAsync("edit_club", true, parameters);
            }
        }

        private async void AddNewClub_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_club", true);
        }

        private async Task LoadData()
        {
            try
            {
                LoadingIndicator.IsRunning = true;
                AddNewClub.IsEnabled = false;
                ClubItems.IsEnabled = false;

                await Task.Delay(750);

                var pretendRdbms = new ObservableCollection<ClubViewModel>() {
                    new()
                    {
                        ID = 1,
                        Name = "Auckland Curry Movement",
                        FoundingFathers = [
                                    new MemberViewModel() { ID = 1, Name = "Alfred Sanchez" },
                                    new MemberViewModel() { ID = 2, Name = "Kyle Scott" },
                                    new MemberViewModel() { ID = 3, Name = "Kaiser Farley" },
                                ]
                    },
                    new()
                    {
                        ID = 2,
                        Name = "Korma Chameleons",
                        FoundingFathers = [
                                    new MemberViewModel() { ID = 4, Name = "Reginald Weber" },
                                    new MemberViewModel() { ID = 5, Name = "Cullen Schmitt" },
                                ]
                    },
                    new()
                    {
                        ID = 3,
                        Name = "Aloo-minati",
                        FoundingFathers = [
                                    new MemberViewModel() { ID = 6, Name = "Kenneth Allen" },
                                    new MemberViewModel() { ID = 7, Name = "Jaxtyn Erickson" },
                                    new MemberViewModel() { ID = 8, Name = "Barrett Herrera" },
                                    new MemberViewModel() { ID = 9, Name = "Lee Dickerson" },
                                ]
                    },
                    new()
                    {
                        ID = 4,
                        Name = "Shashlik our spiced nuts",
                        FoundingFathers = [
                                    new MemberViewModel() { ID = 10, Name = "Edwin Spears" },
                                    new MemberViewModel() { ID = 11, Name = "Luke Wood" },
                                    new MemberViewModel() { ID = 12, Name = "Ben Vega" },
                                    new MemberViewModel() { ID = 13, Name = "Marlowe Ho" },
                                ]
                    },
                    new()
                    {
                        ID = 5,
                        Name = "Vindaloosers",
                        FoundingFathers = [
                                    new MemberViewModel() { ID = 14, Name = "Jaxton Hopkins" },
                                    new MemberViewModel() { ID = 15, Name = "Allen Rangel" },
                                ]
                    },
                    new()
                    {
                        ID = 6,
                        Name = "Pilau Talkers",
                        FoundingFathers = [
                                    new MemberViewModel() { ID = 16, Name = "Fisher Willis" },
                                    new MemberViewModel() { ID = 17, Name = "Terrance Cain" },
                                    new MemberViewModel() { ID = 18, Name = "Alonso Pratt" },
                                    new MemberViewModel() { ID = 19, Name = "Sonny Brandt" },
                                ]
                    },
                };

                _totalPages = pretendRdbms.Count / PageSize;
                if (pretendRdbms.Count % PageSize > 0)
                    _totalPages++;
                _currentPage = Math.Min(Math.Max(0, _currentPage), _totalPages - 1);

                LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;

                MergePageIntoListView(pretendRdbms.Skip(_currentPage * PageSize).Take(PageSize).ToList());

                await Task.Delay(750);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                AddNewClub.IsEnabled = true;
                ClubItems.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<ClubViewModel> pageOfClubs)
        {
            bool changed = false;
            foreach (var club in pageOfClubs)
            {
                var x = _clubs.FirstOrDefault(y => y.ID == club.ID);
                if (x == null)
                {
                    var copy = new ClubViewModel()
                    {
                        ID = club.ID,
                        Name = club.Name,
                        ArchiveReason = club.ArchiveReason,
                        IsArchived = club.IsArchived,
                    };

                    ObservableCollection<MemberViewModel> ffsCopy = [];
                    foreach (var i in club.FoundingFathers)
                        ffsCopy.Add(new MemberViewModel() { ID = i.ID, Name = i.Name });
                    copy.FoundingFathers = ffsCopy;

                    _clubs.Add(copy);
                    changed = true;
                }
            }
            if (changed)
                OnPropertyChanged(nameof(Clubs));
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
