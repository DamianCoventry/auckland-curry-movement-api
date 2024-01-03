using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{

    public partial class AddClubPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private readonly ClubViewModel _club = new();

        public AddClubPage()
        {
            InitializeComponent();
            BindingContext = Club;
        }

        public ClubViewModel Club
        {
            get => _club;
            set
            {
                _club.ID = value.ID;
                _club.Name = value.Name;
                _club.IsArchived = value.IsArchived;
                _club.ArchiveReason = value.ArchiveReason;

                ObservableCollection<MemberViewModel> copy = [];
                foreach (var i in value.FoundingFathers)
                    copy.Add(new MemberViewModel() { ID = i.ID, Name = i.Name });
                _club.FoundingFathers = copy;
            }
        }

        private async void SelectFoundingFathers_Clicked(object sender, EventArgs e)
        {
            string? name = await DisplayPromptAsync("Add Founding Father",
                "How will the brotherhood refer to this gentleman?", "OK",
                "Cancel", "Enter a name", 100);
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
            {
                Club.FoundingFathers.Add(new MemberViewModel()
                {
                    Name = name,
                });
            }
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

            MainThread.BeginInvokeOnMainThread(async () => { await SendAddClubMessage(); });
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task SendAddClubMessage()
        {
            bool saved = false;
            try
            {
                if (SavingIndicator.IsRunning)
                    return;

                var startTime = DateTime.Now;

                ClubName.IsEnabled = false;
                SelectFoundingFathers.IsEnabled = false;
                FoundingFathersListView.IsEnabled = false;
                AcceptChanges.IsEnabled = false;
                DiscardChanges.IsEnabled = false;
                SavingIndicator.IsRunning = true;

                var ffs = new List<acm_models.Member>();
                foreach (var ff in Club.FoundingFathers)
                    ffs.Add(new acm_models.Member() { ID = ff.ID, Name = ff.Name, });

                var club = await AcmService.AddClubAsync(new acm_models.Club()
                {
                    ID = null,
                    Name = Club.Name,
                    ArchiveReason = Club.ArchiveReason,
                    IsArchived = Club.IsArchived,
                    Members = ffs,
                });

                var elapsed = DateTime.Now - startTime;
                if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                    await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);

                saved = club.ID != null;
                if (!saved)
                {
                    var toast = Toast.Make("Unable to add these data as a new club");
                    await toast.Show();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                ClubName.IsEnabled = true;
                SelectFoundingFathers.IsEnabled = true;
                FoundingFathersListView.IsEnabled = true;
                AcceptChanges.IsEnabled = true;
                DiscardChanges.IsEnabled = true;
                SavingIndicator.IsRunning = false;
            }

            if (saved)
                await Shell.Current.GoToAsync("..");
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
