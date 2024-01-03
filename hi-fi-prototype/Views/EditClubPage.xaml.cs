using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Club), "Club")]
    public partial class EditClubPage : ContentPage
	{
        private const int MIN_REFRESH_TIME_MS = 500;
        private readonly ClubViewModel _club = new();
        private int _totalPages = 0;
        private int _currentPage = 0;
        private ObservableCollection<MemberViewModel>? _userSelectedFFs = null;

        public EditClubPage()
		{
			InitializeComponent();
			BindingContext = Club;
		}

        public int PageSize { get; set; } = 10;

        public ClubViewModel Club
		{
			get => _club;
			set
			{
                _club.ID = value.ID;
				_club.Name = value.Name;
                _club.NumMembers = value.NumMembers;
				_club.IsArchived = value.IsArchived;
				_club.ArchiveReason = value.ArchiveReason;
                FoundingFatherItems.BatchBegin();
                _club.FoundingFathers = value.FoundingFathers;
                FoundingFatherItems.BatchCommit();
                _currentPage = 0;
            }
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_userSelectedFFs == null)
                MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfFoundingFathers(); });
            else
                _club.FoundingFathers = CloneMembers(_userSelectedFFs);
        }

        private async void SelectFoundingFathers_Clicked(object sender, EventArgs e)
        {
            SelectMultipleMembersPage page = new()
            {
                SelectedMembers = [.. CloneMembers(_club.FoundingFathers)],
                AcceptFunction = FoundingFathersSelection_Accepted,
            };

            await Navigation.PushAsync(page, true);
        }

        private void FoundingFathersSelection_Accepted(List<MemberViewModel> selectedMembers)
        {
            _userSelectedFFs = CloneMembers(selectedMembers);
        }

        private void RefreshFoundingFathers_Clicked(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfFoundingFathers(); });
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
			await Shell.Current.GoToAsync("..");
        }

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_club.Name))
            {
                var toast = Toast.Make("A name for the club is required");
                await toast.Show();
                return;
            }

            if (_club.FoundingFathers.Count == 0)
            {
                var toast = Toast.Make("At fewest 1 founding father is required");
                await toast.Show();
                return;
            }

            MainThread.BeginInvokeOnMainThread(async () => { await SendSaveClubMessage(); });
        }

        private void ShowHideLoadMoreButton()
        { 
            LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task DownloadSinglePageOfFoundingFathers()
        {
            try
            {
                if (Club == null || Club.ID == null)
                    return;

                var startTime = DateTime.Now;

                LoadMoreButton.IsVisible = false;
                LoadingIndicator.IsRunning = true;
                FoundingFatherItems.IsEnabled = false;
                ClubName.IsEnabled = false;
                SelectFoundingFathers.IsEnabled = false;
                RefreshFoundingFathers.IsEnabled = false;
                DiscardChanges.IsEnabled = false;
                AcceptChanges.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;

                var ffsPageOfData = await AcmService.ListClubFoundingFathersAsync((int)Club.ID, _currentPage * PageSize, PageSize);
                if (ffsPageOfData != null && ffsPageOfData.PageItems != null)
                {
                    _totalPages = ffsPageOfData.TotalPages;
                    MergePageIntoListView(ffsPageOfData.PageItems);
                    ShowHideLoadMoreButton();
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
                ClubName.IsEnabled = true;
                SelectFoundingFathers.IsEnabled = true;
                RefreshFoundingFathers.IsEnabled = true;
                DiscardChanges.IsEnabled = true;
                AcceptChanges.IsEnabled = true;
                FoundingFatherItems.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<acm_models.Member> onePage)
        {
            FoundingFatherItems.BatchBegin();
            foreach (var member in onePage)
            {
                var exists = _club.FoundingFathers.FirstOrDefault(y => y.ID == member.ID);
                if (exists == null)
                {
                    var viewModel = MemberViewModel.FromModel(member);
                    if (viewModel != null)
                        _club.FoundingFathers.Add(viewModel);
                }
            }
            FoundingFatherItems.BatchCommit();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfFoundingFathers(); });
            }
        }

        private void ShowMessageSendingGui(bool show = true)
        {
            ClubName.IsEnabled = !show;
            SelectFoundingFathers.IsEnabled = !show;
            AcceptChanges.IsEnabled = !show;
            DiscardChanges.IsEnabled = !show;
            SavingIndicator.IsRunning = show;
            SavingIndicator.IsVisible = show;
        }

        private async Task SendSaveClubMessage()
        {
            bool saved = false;
            try
            {
                if (SavingIndicator.IsRunning)
                    return;

                ShowMessageSendingGui();

                var startTime = DateTime.Now;

                var model = new acm_models.Club()
                {
                    ID = Club.ID,
                    Name = Club.Name,
                    IsArchived = Club.IsArchived,
                    ArchiveReason = Club.ArchiveReason,
                };

                if (Club.FoundingFathers != null)
                {
                    var ffs = new List<acm_models.Member>();
                    foreach (var ff in Club.FoundingFathers)
                        ffs.Add(new acm_models.Member() { ID = ff.ID, Name = ff.Name });
                    model.Members = ffs;
                }

                saved = await AcmService.UpdateClubAsync(model);

                var elapsed = DateTime.Now - startTime;
                if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                    await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);

                if (!saved)
                {
                    var toast = Toast.Make("Unable to add these data as a new reservation");
                    await toast.Show();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                ShowMessageSendingGui(false);
            }

            if (saved)
                await Shell.Current.GoToAsync("..");
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }





        private static ObservableCollection<MemberViewModel> CloneMembers(ObservableCollection<MemberViewModel> members, int? count = null)
        {
            ObservableCollection<MemberViewModel> clone = [];

            int theCount = Math.Min(members.Count, count != null ? count.Value : members.Count);
            for (int i = 0; i < theCount; ++i)
            {
                clone.Add(new MemberViewModel()
                {
                    ID = members[i].ID,
                    Name = members[i].Name,
                    ArchiveReason = members[i].ArchiveReason,
                    IsArchived = members[i].IsArchived,
                });
            }

            return clone;
        }

        private static ObservableCollection<MemberViewModel> CloneMembers(List<MemberViewModel> members, int? count = null)
        {
            ObservableCollection<MemberViewModel> clone = [];

            int theCount = Math.Min(members.Count, count != null ? count.Value : members.Count);
            for (int i = 0; i < theCount; ++i)
            {
                clone.Add(new MemberViewModel()
                {
                    ID = members[i].ID,
                    Name = members[i].Name,
                    ArchiveReason = members[i].ArchiveReason,
                    IsArchived = members[i].IsArchived,
                });
            }

            return clone;
        }
    }
}
