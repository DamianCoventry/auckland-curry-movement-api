using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace hi_fi_prototype.Views
{
    public partial class ManageLevelsPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<LevelViewModel> _levels = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageLevelsPage()
        {
            InitializeComponent();
            BindingContext = this;

            DeleteLevelCommand = new Command<int>(
                execute: async (int id) =>
                {
                    bool yes = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this level?", "Yes", "No");
                    if (yes)
                        await SendDeleteLevelMessage(id);
                },
                canExecute: (int id) =>
                {
                    var x = Levels.FirstOrDefault(x => x.ID == id);
                    return x != null && !x.IsDeleting;
                });
        }

        public int PageSize { get; set; } = 10;

        public ICommand DeleteLevelCommand { private set; get; }

        public ObservableCollection<LevelViewModel> Levels
        {
            get => _levels;
            set
            {
                _levels = [];
                foreach (var level in value)
                {
                    var copy = new LevelViewModel()
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
                //OnPropertyChanged(nameof(Levels));
                (MainScrollView as IView).InvalidateMeasure();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfLevels(); });
        }

        private void RefreshLevelsList_Clicked(object sender, EventArgs e)
        {
            Levels = [];
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfLevels(); });
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

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }


        private async Task DownloadSinglePageOfLevels()
        {
            try
            {
                if (LoadingIndicator.IsRunning)
                    return;

                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                AddNewLevel.IsEnabled = false;
                LevelItems.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;
                RefreshLevelsList.IsEnabled = false;

                var pageOfData = await AcmService.ListLevelsAsync(_currentPage * PageSize, PageSize);
                if (pageOfData != null && pageOfData.PageItems != null)
                {
                    _totalPages = pageOfData.TotalPages;
                    LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
                    MergePageIntoListView(pageOfData.PageItems);
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
                AddNewLevel.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
                LevelItems.IsEnabled = true;
                RefreshLevelsList.IsEnabled = true;
            }
        }

        private async Task SendDeleteLevelMessage(int id)
        {
            bool deleteSucceeded = false;
            try
            {
                var startTime = DateTime.Now;

                var deleting = Levels.FirstOrDefault(x => x.ID == id);
                if (deleting != null)
                    deleting.IsDeleting = true; // Shows an activity indicator

                AddNewLevel.IsEnabled = false;
                LevelItems.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;
                RefreshLevelsList.IsEnabled = false;

                deleteSucceeded = await AcmService.DeleteLevelAsync(id);
                if (deleteSucceeded)
                {
                    var elapsed = DateTime.Now - startTime;
                    if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS * 2)
                        await Task.Delay(MIN_REFRESH_TIME_MS * 2 - elapsed.Milliseconds);

                    if (deleting != null)
                        Levels.Remove(deleting);
                }
                else
                    await DisplayAlert("Delete Failed", $"Unable to delete the level with the {id}.", "Close");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                AddNewLevel.IsEnabled = true;
                LevelItems.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
                RefreshLevelsList.IsEnabled = true;
            }

            if (deleteSucceeded)
                await DownloadSinglePageOfLevels();
        }

        private void MergePageIntoListView(List<acm_models.Level> pageOfLevels)
        {
            //LevelItems.BatchCommit();
            foreach (var level in pageOfLevels)
            {
                var existing = _levels.FirstOrDefault(y => y.ID == level.ID);
                if (existing == null)
                {
                    var vm = LevelViewModel.FromModel(level);
                    if (vm != null)
                        _levels.Add(vm);
                }
                else
                {
                    existing.ID = level.ID;
                    existing.Name = level.Name;
                    existing.Description = level.Description;
                    existing.RequiredAttendances = level.RequiredAttendances;
                    existing.ArchiveReason = level.ArchiveReason;
                    existing.IsArchived = level.IsArchived;
                }
            }
            //LevelItems.BatchCommit();
            (MainScrollView as IView).InvalidateArrange();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfLevels(); });
            }
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
