using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

public partial class AddExemption : ContentPage
{
    private readonly Exemption _exemption = new();

    public AddExemption()
	{
		InitializeComponent();
        BindingContext = Exemption;
    }

    public Exemption Exemption
    {
        get => _exemption;
        set
        {
            _exemption.ID = value.ID;
            _exemption.FoundingFatherID = value.FoundingFatherID;
            _exemption.MemberID = value.MemberID;
            _exemption.Date = value.Date;
            _exemption.ShortReason = value.ShortReason;
            _exemption.LongReason = value.LongReason;
            _exemption.IsArchived = value.IsArchived;
            _exemption.ArchiveReason = value.ArchiveReason;
        }
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FoundingFather.Text))
        {
            var toast = Toast.Make("A founding father is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(MemberName.Text))
        {
            var toast = Toast.Make("A member is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(ShortReason.Text))
        {
            var toast = Toast.Make("A short reason is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (await AddExistingExemptionAsync())
            await Shell.Current.GoToAsync("//manage_exemptions");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async void SelectFoundingFather_Clicked(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new()
        {
            { "ClubID", 1 }, // TODO: Where should we get this from?
            { "SelectedMember", new SelectedMember()
                {
                    IsSelected = _exemption.FoundingFatherID > 0,
                    IsFoundingFather = true,
                    Member = new Member() { ID = _exemption.FoundingFatherID }
                }
            },
        };

        await Navigation.PushAsync(new SelectOneMember(parameters, x =>
            {
                if (x.Member.ID != null)
                {
                    _exemption.FoundingFatherID = (int)x.Member.ID;
                    _exemption.FoundingFather = x.Member;
                }
            }), true);
    }

    private async void SelectMember_Clicked(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new()
        {
            { "ClubID", 1 }, // TODO: Where should we get this from?
            { "SelectedMember", new SelectedMember()
                {
                    IsSelected = _exemption.MemberID > 0,
                    IsFoundingFather = false,
                    Member = new Member() { ID = _exemption.MemberID }
                }
            },
        };

        await Navigation.PushAsync(new SelectOneMember(parameters, x =>
            {
                if (x.Member.ID != null)
                {
                    _exemption.MemberID = (int)x.Member.ID;
                    _exemption.Member = x.Member;
                }
            }), true);
    }

    private async Task<bool> AddExistingExemptionAsync()
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
            await AcmService.AddExemptionAsync(new acm_models.Exemption
            {
                FoundingFatherID = Exemption.FoundingFatherID,
                MemberID = Exemption.MemberID,
                Date = Exemption.Date,
                ShortReason = Exemption.ShortReason,
                LongReason = Exemption.LongReason
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
