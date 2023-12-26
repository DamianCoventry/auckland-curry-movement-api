using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Club), "Club")]
    public partial class EditClubPage : ContentPage
	{
		private readonly ClubViewModel _club = new();
        private ObservableCollection<MemberViewModel> _pretendRdbms = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        private ObservableCollection<MemberViewModel>? _selectedPretendRdbms = null;

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
                LoadMoreButton.IsVisible = false;

                _club.ID = value.ID;
				_club.Name = value.Name;
				_club.IsArchived = value.IsArchived;
				_club.ArchiveReason = value.ArchiveReason;

                if (_selectedPretendRdbms != null)
                {
                    _pretendRdbms = CloneMembers(_selectedPretendRdbms);
                    _selectedPretendRdbms = null;
                }
                else
                    _pretendRdbms = CloneMembers(value.FoundingFathers);

                _club.FoundingFathers = CloneMembers(_pretendRdbms, PageSize);


                RecalculateTotalPages();
                ShowHideLoadMoreButton();
            }
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
        }

        private async void SelectFoundingFathers_Clicked(object sender, EventArgs e)
        {
            _selectedPretendRdbms = null;

            SelectMultipleMembersPage page = new()
            {
                SelectedMembers = [.. CloneMembers(_club.FoundingFathers)],
                AcceptFunction = FoundingFathersSeletction_Accepted,
            };

            await Navigation.PushAsync(page, true);
        }

        private void FoundingFathersSeletction_Accepted(List<MemberViewModel> selectedMembers)
        {
            _selectedPretendRdbms = CloneMembers(selectedMembers);
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

            ClubName.IsEnabled = false;
            SelectFoundingFathers.IsEnabled = false;
            AcceptChanges.IsEnabled = false;
            DiscardChanges.IsEnabled = false;
			SavingIndicator.IsRunning = true;
			SavingIndicator.IsVisible = true;
			await Task.Delay(1500); // Fake saving
            await Shell.Current.GoToAsync("..");
        }

        private void RecalculateTotalPages()
        {
            _totalPages = _pretendRdbms.Count / PageSize;
            if (_pretendRdbms.Count % PageSize > 0)
                _totalPages++;

            _currentPage = Math.Min(Math.Max(0, _currentPage), _totalPages - 1);
        }

        private void ShowHideLoadMoreButton()
        { 
            LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
        }

        private async Task LoadData()
        {
            try
            {
                LoadMoreButton.IsVisible = false;
                LoadingIndicator.IsRunning = true;
                ClubName.IsEnabled = false;
                SelectFoundingFathers.IsEnabled = false;
                FoundingFatherItems.IsEnabled = false;
                DiscardChanges.IsEnabled = false;
                AcceptChanges.IsEnabled = false;

                await Task.Delay(750);
                MergePageIntoListView(_pretendRdbms.Skip(_currentPage * PageSize).Take(PageSize).ToList());

                await Task.Delay(750);
                ShowHideLoadMoreButton();
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
                FoundingFatherItems.IsEnabled = true;
                DiscardChanges.IsEnabled = true;
                AcceptChanges.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<MemberViewModel> onePage)
        {
            bool changed = false;
            foreach (var member in onePage)
            {
                var x = _club.FoundingFathers.FirstOrDefault(y => y.ID == member.ID);
                if (x == null)
                {
                    var copy = new MemberViewModel()
                    {
                        ID = member.ID,
                        ArchiveReason = member.ArchiveReason,
                        IsArchived = member.IsArchived,
                        Name = member.Name,
                    };
                    _club.FoundingFathers.Add(copy);
                    changed = true;
                }
            }
            if (changed)
                OnPropertyChanged(nameof(Club));
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
            }
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
                    ArchiveReason = members[i].ArchiveReason,
                    IsArchived = members[i].IsArchived,
                    Name = members[i].Name,
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
                    ArchiveReason = members[i].ArchiveReason,
                    IsArchived = members[i].IsArchived,
                    Name = members[i].Name,
                });
            }

            return clone;
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
