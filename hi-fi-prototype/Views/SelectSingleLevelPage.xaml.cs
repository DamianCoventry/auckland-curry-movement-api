using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class SelectSingleLevelPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<LevelViewModel> _levels = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public SelectSingleLevelPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Action<LevelViewModel>? AcceptFunction { get; set; } = null;
        public Action? CancelFunction { get; set; } = null;
        public LevelViewModel? SelectedLevel { get; set; } = null;
        private LevelViewModel? OriginalSelection { get; set; } = null;
        public int PageSize { get; set; } = 10;

        public ObservableCollection<LevelViewModel> Levels
        {
            get => _levels;
            set
            {
                _levels = [];
                foreach (var level in value)
                {
                    _levels.Add(new LevelViewModel()
                    {
                        ID = level.ID,
                        Name = level.Name,
                        ArchiveReason = level.ArchiveReason,
                        IsArchived = level.IsArchived,
                    });
                }
                OnPropertyChanged(nameof(Levels));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (SelectedLevel != null)
            {
                OriginalSelection = new LevelViewModel()
                {
                    ID = SelectedLevel.ID,
                    Name = SelectedLevel.Name,
                    ArchiveReason = SelectedLevel.ArchiveReason,
                    IsArchived = SelectedLevel.IsArchived,
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

                if (OriginalSelection != null)
                {
                    foreach (var level in Levels)
                    {
                        if (OriginalSelection != null && OriginalSelection.ID == level.ID && OriginalSelection.Name == level.Name)
                        {
                            OriginalSelection = null;
                            LevelItems.SelectedItem = level;
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
            if (LevelItems.SelectedItem == null)
            {
                var toast = Toast.Make("Please select a level");
                await toast.Show();
                return;
            }

            if (LevelItems.SelectedItem is LevelViewModel x)
            {
                SelectedLevel = x;
                AcceptFunction?.Invoke(SelectedLevel);
                await Navigation.PopAsync();
            }
        }

        private void MergePageIntoListView(List<LevelViewModel> pageOfLevels)
        {
            LevelItems.BatchBegin();
            foreach (var level in pageOfLevels)
            {
                var x = _levels.FirstOrDefault(y => y.ID == level.ID);
                if (x == null)
                {
                    _levels.Add(new LevelViewModel()
                    {
                        ID = level.ID,
                        Name = level.Name,
                        ArchiveReason = level.ArchiveReason,
                        IsArchived = level.IsArchived,
                    });
                }
            }
            LevelItems.BatchCommit();
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
