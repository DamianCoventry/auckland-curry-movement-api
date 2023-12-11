using acm_mobile_app.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

[QueryProperty(nameof(Exemption), "Exemption")]
public partial class EditExemption : ContentPage
{
    private readonly ViewModels.Exemption _exemption = new();

    public EditExemption()
	{
		InitializeComponent();
        BindingContext = Exemption;
    }

    public ViewModels.Exemption Exemption
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

        if (await EditExistingExemptionAsync())
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
}
