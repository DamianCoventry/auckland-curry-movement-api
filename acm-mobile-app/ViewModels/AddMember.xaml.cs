using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

public partial class AddMember : ContentPage
{
    private Member _member = new();

    public AddMember()
    {
        InitializeComponent();
        BindingContext = _member;
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MemberName.Text))
        {
            var toast = Toast.Make("A name is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (await AddNewMemberAsync())
            await Shell.Current.GoToAsync("//manage_members");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
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
                        ID = _member.ID,
                        Name = _member.Name,
                        SponsorID = _member.SponsorID,
                        Sponsor = _member.Sponsor,
                        CurrentLevelID = _member.CurrentLevelID,
                        CurrentLevel = _member.CurrentLevel,
                        AttendanceCount = _member.AttendanceCount,
                        IsArchived = _member.IsArchived,
                        ArchiveReason = _member.ArchiveReason,
                    }
                }
            },
        };

        await Navigation.PushAsync(new SelectOneMember(parameters, x =>
        {
            if (x.Member.ID != null)
                _member = x.Member;
        }), true);
    }

    private async Task<bool> AddNewMemberAsync()
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
            await AcmService.AddMemberAsync(new acm_models.Member
            {
                Name = _member.Name,
                SponsorID = _member.SponsorID,
                CurrentLevelID = _member.CurrentLevelID,
                AttendanceCount = _member.AttendanceCount,
                IsArchived = _member.IsArchived,
                ArchiveReason = _member.ArchiveReason,
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
}
