using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Member), "Member")]
    public partial class EditMemberPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private readonly MemberViewModel _member = new();
        private MemberViewModel? _sponsorSelection = null;
        private LevelViewModel? _levelSelection = null;

        public EditMemberPage()
        {
            InitializeComponent();
            BindingContext = Member;
        }

        public MemberViewModel Member
        {
            get => _member;
            set
            {
                _member.ID = value.ID;
                _member.Name = value.Name;
                _member.AttendanceCount = value.AttendanceCount;
                _member.IsArchived = value.IsArchived;
                _member.ArchiveReason = value.ArchiveReason;

                if (_sponsorSelection != null)
                {
                    _member.SponsorID = _sponsorSelection.ID;
                    _member.Sponsor = _sponsorSelection;
                }
                else
                {
                    _member.SponsorID = value.SponsorID;
                    _member.Sponsor = value.Sponsor;
                }

                if (_levelSelection != null && _levelSelection.ID != null)
                {
                    _member.CurrentLevelID = (int)_levelSelection.ID;
                    _member.CurrentLevel = _levelSelection;
                }
                else
                {
                    _member.CurrentLevelID = value.CurrentLevelID;
                    _member.CurrentLevel = value.CurrentLevel;
                }
            }
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_member.Name))
            {
                var toast = Toast.Make("A name for the member is required");
                await toast.Show();
                return;
            }

            MemberName.IsEnabled = false;
            ChooseSponsor.IsEnabled = false;
            ChooseLevel.IsEnabled = false;
            AcceptChanges.IsEnabled = false;
            DiscardChanges.IsEnabled = false;
            SavingIndicator.IsRunning = true;
            SavingIndicator.IsVisible = true;

            MainThread.BeginInvokeOnMainThread(async () => { await SendSaveMemberMessage(); });
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }

        private async void ChooseSponsor_Clicked(object sender, EventArgs e)
        {
            MemberViewModel? copy = null;

            if (_member.Sponsor != null)
            {
                copy = new MemberViewModel()
                {
                    ID = _member.Sponsor.ID,
                    Name = _member.Sponsor.Name,
                    SponsorID = null,
                    Sponsor = null,
                    CurrentLevelID = _member.Sponsor.CurrentLevelID,
                    CurrentLevel = _member.Sponsor.CurrentLevel,
                    AttendanceCount = _member.Sponsor.AttendanceCount,
                    IsArchived = _member.Sponsor.IsArchived,
                    ArchiveReason = _member.Sponsor.ArchiveReason,
                };
            }

            SelectSingleMemberPage selectSingleMemberPage = new()
            {
                SelectedMember = copy,
                AcceptFunction = SponsorSelection_Accepted,
            };

            await Navigation.PushAsync(selectSingleMemberPage);
        }

        private void SponsorSelection_Accepted(MemberViewModel model)
        {
            _sponsorSelection = new MemberViewModel()
            {
                ID = model.ID,
                Name = model.Name,
                SponsorID = model.SponsorID,
                Sponsor = model.Sponsor,
                CurrentLevelID = model.CurrentLevelID,
                CurrentLevel = model.CurrentLevel,
                AttendanceCount = model.AttendanceCount,
                IsArchived = model.IsArchived,
                ArchiveReason = model.ArchiveReason,
            };
        }

        private async void ChooseLevel_Clicked(object sender, EventArgs e)
        {
            LevelViewModel? copy = null;

            if (_member.CurrentLevel != null)
            {
                copy = new LevelViewModel()
                {
                    ID = _member.CurrentLevelID,
                    RequiredAttendances = _member.CurrentLevel.RequiredAttendances,
                    Name = _member.CurrentLevel.Name,
                    Description = _member.CurrentLevel.Description,
                    IsArchived = _member.CurrentLevel.IsArchived,
                    ArchiveReason = _member.CurrentLevel.ArchiveReason,
                };
            }

            SelectSingleLevelPage selectSingleLevelPage = new()
            {
                SelectedLevel = copy,
                AcceptFunction = CurrentLevelSelection_Accepted,
            };

            await Navigation.PushAsync(selectSingleLevelPage);
        }

        private void CurrentLevelSelection_Accepted(LevelViewModel model)
        {
            _levelSelection = new LevelViewModel()
            {
                ID = model.ID,
                RequiredAttendances = model.RequiredAttendances,
                Name = model.Name,
                Description = model.Description,
                IsArchived = model.IsArchived,
                ArchiveReason = model.ArchiveReason,
            };
        }

        private void ShowMessageSendingGui(bool show = true)
        {
            MemberName.IsEnabled = !show;
            ChooseSponsor.IsEnabled = !show;
            ChooseLevel.IsEnabled = !show;
            AcceptChanges.IsEnabled = !show;
            DiscardChanges.IsEnabled = !show;
            SavingIndicator.IsRunning = show;
            SavingIndicator.IsVisible = show;
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task SendSaveMemberMessage()
        {
            bool saved = false;
            try
            {
                if (SavingIndicator.IsRunning)
                    return;

                ShowMessageSendingGui();

                var startTime = DateTime.Now;

                var model = new acm_models.Member()
                {
                    ID = Member.ID,
                    Name = Member.Name,
                    SponsorID = Member.SponsorID,
                    CurrentLevelID = Member.CurrentLevelID,
                    AttendanceCount = Member.AttendanceCount,
                    IsArchived = Member.IsArchived,
                    ArchiveReason = Member.ArchiveReason,
                };

                if (Member.Sponsor != null)
                    model.Sponsor = new acm_models.Member() { ID = Member.SponsorID, Name = Member.Sponsor.Name };
                if (Member.CurrentLevel != null)
                    model.CurrentLevel = new acm_models.Level() { ID = Member.CurrentLevelID, Name = Member.CurrentLevel.Name };

                saved = await AcmService.UpdateMemberAsync(model);

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
    }
}
