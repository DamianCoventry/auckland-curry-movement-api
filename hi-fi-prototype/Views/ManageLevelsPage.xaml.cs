using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageLevelsPage : ContentPage
    {
        private ObservableCollection<LevelViewModel> _levels = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageLevelsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int PageSize { get; set; } = 10;

        public ObservableCollection<LevelViewModel> Levels
        {
            get => _levels;
            set
            {
                _levels = [];
                foreach (var level in value)
                {
                    var copy = new LevelViewModel
                    {
                        ID = level.ID,
                        Name = level.Name,
                        Description = level.Description,
                        RequiredAttendances = level.RequiredAttendances,
                        ArchiveReason = level.ArchiveReason,
                        IsArchived = level.IsArchived,
                    };
                    _levels.Add(copy);
                }
                OnPropertyChanged(nameof(Levels));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
        }

        private async void LevelItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is LevelViewModel levelViewModel)
            {
                Dictionary<string, object> parameters = new() { { "Level", levelViewModel } };
                await Shell.Current.GoToAsync("edit_level", true, parameters);
            }
        }

        private async void AddNewLevel_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add_level", true);
        }

        private async Task LoadData()
        {
            try
            {
                LoadingIndicator.IsRunning = true;
                AddNewLevel.IsEnabled = false;
                LevelItems.IsEnabled = false;

                await Task.Delay(750);

                var pretendRdbms = new ObservableCollection<LevelViewModel>() {
                    new()
                    {
                        ID = 1,
                        Name = "Nausikhie",
                        Description = "A level new to and inexperienced in the club.",
                        RequiredAttendances = 0,
                    },
                    new()
                    {
                        ID = 2,
                        Name = "Shuruaatee",
                        Description = "A level just starting to learn curry and take part in the club.",
                        RequiredAttendances = 10,
                    },
                    new()
                    {
                        ID = 3,
                        Name = "Pratiyogee",
                        Description = "A level who actively takes part in the club.",
                        RequiredAttendances = 25,
                    },
                    new()
                    {
                        ID = 4,
                        Name = "Peshevar",
                        Description = "Engaged in the club as one's main vocation rather than as a pastime.",
                        RequiredAttendances = 50,
                    },
                    new()
                    {
                        ID = 6,
                        Name = "Visheshagy",
                        Description = "A level who is very knowledgeable about curry and the club.",
                        RequiredAttendances = 100,
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
                AddNewLevel.IsEnabled = true;
                LevelItems.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<LevelViewModel> pageOfLevels)
        {
            bool changed = false;
            foreach (var level in pageOfLevels)
            {
                var x = _levels.FirstOrDefault(y => y.ID == level.ID);
                if (x == null)
                {
                    _levels.Add(new LevelViewModel()
                    {
                        ID = level.ID,
                        Name = level.Name,
                        Description = level.Description,
                        RequiredAttendances = level.RequiredAttendances,
                        ArchiveReason = level.ArchiveReason,
                        IsArchived = level.IsArchived,
                    });
                    changed = true;
                }
            }
            if (changed)
                OnPropertyChanged(nameof(Levels));
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
