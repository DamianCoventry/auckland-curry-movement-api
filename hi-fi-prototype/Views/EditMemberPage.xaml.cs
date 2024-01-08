using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Membership), "Membership")]
    public partial class EditMemberPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private readonly MembershipViewModel _membership = new() { Member = new MemberViewModel() };
        private MembershipViewModel? _sponsorSelection = null;
        private LevelViewModel? _levelSelection = null;

        public EditMemberPage()
        {
            InitializeComponent();
            BindingContext = Membership;
        }

        public MembershipViewModel Membership
        {
            get => _membership;
            set
            {
                if (value.Member != null)
                    _membership.Member = new MemberViewModel() { ID = value.Member.ID, Name = value.Member.Name, };

                _membership.MemberID = value.MemberID;
                _membership.AttendanceCount = value.AttendanceCount;
                _membership.IsAdmin = value.IsAdmin;
                _membership.IsFoundingFather = value.IsFoundingFather;
                _membership.IsAuditor = value.IsAuditor;
                _membership.IsArchived = value.IsArchived;
                _membership.ArchiveReason = value.ArchiveReason;

                if (_sponsorSelection != null)
                {
                    _membership.SponsorID = _sponsorSelection.MemberID;
                    _membership.Sponsor = _sponsorSelection;
                }
                else
                {
                    _membership.SponsorID = value.SponsorID;
                    _membership.Sponsor = value.Sponsor;
                }

                if (_levelSelection != null && _levelSelection.ID != null)
                {
                    _membership.LevelID = (int)_levelSelection.ID;
                    _membership.Level = _levelSelection;
                }
                else
                {
                    _membership.LevelID = value.LevelID;
                    _membership.Level = value.Level;
                }
            }
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_membership.Member?.Name))
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
            MembershipViewModel? copy = null;

            if (_membership.Sponsor != null)
            {
                var sponsorMember = _membership.Sponsor.Member;
                if (sponsorMember != null)
                {
                    copy = new MembershipViewModel()
                    {
                        ClubID = _membership.ClubID,
                        SponsorID = null,
                        Sponsor = null,
                        LevelID = _membership.Sponsor.LevelID,
                        Level = _membership.Sponsor.Level,
                        AttendanceCount = _membership.Sponsor.AttendanceCount,
                        IsAdmin = _membership.Sponsor.IsAdmin,
                        IsFoundingFather = _membership.Sponsor.IsFoundingFather,
                        IsAuditor = _membership.Sponsor.IsAuditor,
                        IsArchived = _membership.Sponsor.IsArchived,
                        ArchiveReason = _membership.Sponsor.ArchiveReason,
                        Member = new MemberViewModel()
                        {
                            ID = sponsorMember.ID,
                            Name = sponsorMember.Name,
                            IsArchived = sponsorMember.IsArchived,
                            ArchiveReason = sponsorMember.ArchiveReason,
                        },
                    };
                }
            }

            SelectSingleMemberPage selectSingleMemberPage = new()
            {
                SelectedMember = copy,
                AcceptFunction = SponsorSelection_Accepted,
            };

            await Navigation.PushAsync(selectSingleMemberPage);
        }

        private void SponsorSelection_Accepted(MembershipViewModel model)
        {
            _sponsorSelection = new MembershipViewModel()
            {
                ClubID = model.ClubID,
                SponsorID = model.SponsorID,
                Sponsor = model.Sponsor,
                LevelID = model.LevelID,
                Level = model.Level,
                AttendanceCount = model.AttendanceCount,
                IsAdmin = model.IsAdmin,
                IsFoundingFather = model.IsFoundingFather,
                IsAuditor = model.IsAuditor,
                IsArchived = model.IsArchived,
                ArchiveReason = model.ArchiveReason,
            };

            if (model.Member != null)
            {
                _sponsorSelection.Member = new MemberViewModel()
                {
                    ID = model.Member.ID,
                    Name = model.Member.Name,
                    IsArchived = model.Member.IsArchived,
                    ArchiveReason = model.Member.ArchiveReason,
                };
            }
        }

        private async void ChooseLevel_Clicked(object sender, EventArgs e)
        {
            LevelViewModel? copy = null;

            if (_membership.Level != null)
            {
                copy = new LevelViewModel()
                {
                    ID = _membership.LevelID,
                    RequiredAttendances = _membership.Level.RequiredAttendances,
                    Name = _membership.Level.Name,
                    Description = _membership.Level.Description,
                    IsArchived = _membership.Level.IsArchived,
                    ArchiveReason = _membership.Level.ArchiveReason,
                };
            }

            SelectSingleLevelPage selectSingleLevelPage = new()
            {
                SelectedLevel = copy,
                AcceptFunction = LevelSelection_Accepted,
            };

            await Navigation.PushAsync(selectSingleLevelPage);
        }

        private void LevelSelection_Accepted(LevelViewModel model)
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

                var model = new acm_models.Membership()
                {
                    ClubID = Membership.ClubID,
                    SponsorID = Membership.SponsorID,
                    LevelID = Membership.LevelID,
                    AttendanceCount = Membership.AttendanceCount,
                    IsAdmin = Membership.IsAdmin,
                    IsFoundingFather = Membership.IsFoundingFather,
                    IsAuditor = Membership.IsAuditor,
                    IsArchived = Membership.IsArchived,
                    ArchiveReason = Membership.ArchiveReason,
                };

                if (Membership.Member != null)
                    model.Member = new acm_models.Member() { ID = Membership.Member.ID, Name = Membership.Member.Name };
                if (Membership.Level != null)
                    model.Level = new acm_models.Level() { ID = Membership.Level.ID, Name = Membership.Level.Name };
                if (Membership.Sponsor != null && Membership.Sponsor.Member != null)
                {
                    model.Sponsor = new acm_models.Membership()
                    {
                        ClubID = Membership.ClubID,
                        Member = new acm_models.Member() { ID = Membership.Sponsor.Member.ID, Name = Membership.Sponsor.Member.Name }
                    };
                }

                if (Membership.Member != null && Membership.Member.ID != null)
                    saved = await AcmService.UpdateClubMembershipAsync(Membership.ClubID, (int)Membership.Member.ID, model);

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
