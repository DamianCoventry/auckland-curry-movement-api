using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

[QueryProperty(nameof(Exemption), "Exemption")]
public partial class EditExemption : ContentPage
{
    private readonly Exemption _exemption = new();
    private Member? _foundingFather = null;
    private Member? _member = null;

    public EditExemption()
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

        if (await EditExistingExemptionAsync())
            await Shell.Current.GoToAsync("//manage_exemptions");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(async () => { await LoadExemptionData(); });
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
                    _foundingFather = new Member() { ID = x.Member.ID, Name = x.Member.Name };
                    CopyLocalStateToGui();
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
                    _member = new Member() { ID = x.Member.ID, Name = x.Member.Name };
                    CopyLocalStateToGui();
                }
            }), true);
    }

    private async Task LoadExemptionData()
    {
        try
        {
            if (LoadingIndicator.IsRunning || _exemption.ID == null)
                return;

            LoadingIndicator.IsRunning = true;

            await Task.Delay(150);
            var model = await AcmService.GetExemptionAsync((int)_exemption.ID);

            await Task.Delay(150);
            var x = Exemption.FromModel(model);
            if (x != null)
            {
                Exemption = x;

                if (_foundingFather == null && x.FoundingFather != null)
                    _foundingFather = new Member() { ID = x.FoundingFather.ID, Name = x.FoundingFather.Name };

                if (_member == null && x.Member != null)
                    _member = new Member() { ID = x.Member.ID, Name = x.Member.Name };

                CopyLocalStateToGui();
            }
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

    private async Task<bool> EditExistingExemptionAsync()
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
            await AcmService.UpdateExemptionAsync(new acm_models.Exemption
            {
                ID = Exemption.ID,
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

    private void CopyLocalStateToGui()
    {
        if (_foundingFather != null)
        {
            _exemption.FoundingFatherID = _foundingFather.ID != null ? (int)_foundingFather.ID : 0;
            _exemption.FoundingFather = _foundingFather;
        }
        if (_member != null)
        {
            _exemption.MemberID = _member.ID != null ? (int)_member.ID : 0;
            _exemption.Member = _member;
        }
    }
}
