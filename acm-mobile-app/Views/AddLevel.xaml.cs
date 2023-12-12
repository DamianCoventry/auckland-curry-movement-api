using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

public partial class AddLevel : ContentPage
{
    private readonly Level _level = new();

    public AddLevel()
    {
        InitializeComponent();
        BindingContext = _level;
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LevelName.Text))
        {
            var toast = Toast.Make("A name is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(LevelDescription.Text))
        {
            var toast = Toast.Make("A description is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(RequiredAttendances.Text))
        {
            var toast = Toast.Make("The number of required attendances is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (await AddNewLevelAsync())
            await Shell.Current.GoToAsync("//manage_levels");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async Task<bool> AddNewLevelAsync()
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
            await AcmService.AddLevelAsync(new acm_models.Level
            {
                Name = _level.Name,
                Description = _level.Description,
                RequiredAttendances = _level.RequiredAttendances,
                IsArchived = _level.IsArchived,
                ArchiveReason = _level.ArchiveReason,
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
