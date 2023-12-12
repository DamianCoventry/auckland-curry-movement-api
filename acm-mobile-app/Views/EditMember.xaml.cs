using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

[QueryProperty(nameof(Member), "Member")]
public partial class EditMember : ContentPage
{
    private readonly Member _member = new();
    private Member? _sponsor = null;

    public EditMember()
    {
        InitializeComponent();
        BindingContext = Member;
    }

    public Member Member
    {
        get => _member;
        set
        {
            _member.ID = value.ID;
            _member.Name = value.Name;
            _member.SponsorID = value.SponsorID;
            _member.Sponsor = value.Sponsor;
            _member.CurrentLevelID = value.CurrentLevelID;
            _member.CurrentLevel = value.CurrentLevel;
            _member.AttendanceCount = value.AttendanceCount;
            _member.IsArchived = value.IsArchived;
            _member.ArchiveReason = value.ArchiveReason;
        }
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MemberName.Text))
        {
            var toast = Toast.Make("A name is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (await EditExistingMemberAsync())
            await Shell.Current.GoToAsync("//manage_members");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(async () => { await LoadMemberData(); });
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async void SelectSponsor_Clicked(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new()
        {
            { "ClubID", 1 }, // TODO: Where should we get this from?
            { "SelectedMember", new SelectedMember()
                {
                    IsSelected = _member.ID != null && (int)_member.ID > 0,
                    IsFoundingFather = false,
                    Member = new Member()
                    {
                        ID = _member.SponsorID,
                        Name = _member.Sponsor != null ? _member.Sponsor.Name : ""
                    }
                }
            },
        };

        await Navigation.PushAsync(new SelectOneMember(parameters, x =>
        {
            if (x.Member.ID != null)
            {
                _sponsor = new Member() { ID = x.Member.ID, Name = x.Member.Name };
                CopyLocalStateToGui();
            }
        }), true);
    }

    private async Task LoadMemberData()
    {
        try
        {
            if (LoadingIndicator.IsRunning || _member.ID == null)
                return;

            LoadingIndicator.IsRunning = true;

            await Task.Delay(150);
            var memberModel = await AcmService.GetMemberAsync((int)_member.ID);

            var x = Member.FromModel(memberModel);
            if (x != null)
            {
                Member = x;

                if (_sponsor == null && x.Sponsor != null)
                    _sponsor = new Member() { ID = x.Sponsor.ID, Name = x.Sponsor.Name };

                CopyLocalStateToGui();
            }

            await Task.Delay(150);
            var levelModel = await AcmService.GetLevelAsync(memberModel.CurrentLevelID);
            if (Member != null)
                Member.CurrentLevel = Level.FromModel(levelModel);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Close");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
        }
    }

    private async Task<bool> EditExistingMemberAsync()
    {
        bool rv = false;
        try
        {
            if (SavingIndicator.IsRunning)
                return rv;

            SavingIndicator.IsRunning = true;
            OkButton.IsVisible = false;
            CancelButton.IsVisible = false;

            await Task.Delay(150);
            await AcmService.UpdateMemberAsync(new acm_models.Member
            {
                ID = Member.ID,
                Name = Member.Name,
                SponsorID = Member.SponsorID,
                CurrentLevelID = Member.CurrentLevelID,
                AttendanceCount = Member.AttendanceCount,
                IsArchived = Member.IsArchived,
                ArchiveReason = Member.ArchiveReason,
            });

            await Task.Delay(150);
            rv = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Close");
        }
        finally
        {
            SavingIndicator.IsRunning = false;
            OkButton.IsVisible = true;
            CancelButton.IsVisible = true;
        }
        return rv;
    }

    private void CopyLocalStateToGui()
    {
        if (_sponsor != null)
        {
            _member.SponsorID = _sponsor.ID;
            _member.Sponsor = new Member() { ID = _sponsor.ID, Name = _sponsor.Name };
        }
    }
}
