using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

[QueryProperty(nameof(Level), "Level")]
public partial class EditLevel : ContentPage
{
    private readonly Level _level = new();

    public EditLevel()
    {
        InitializeComponent();
        BindingContext = Level;
    }

    public Level Level
    {
        get => _level;
        set
        {
            _level.ID = value.ID;
            _level.Name = value.Name;
            _level.RequiredAttendances = value.RequiredAttendances;
            _level.Description = value.Description;
            _level.IsArchived = value.IsArchived;
            _level.ArchiveReason = value.ArchiveReason;
        }
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

        if (await EditExistingLevelAsync())
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

    private async Task<bool> EditExistingLevelAsync()
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
            await AcmService.UpdateLevelAsync(new acm_models.Level
            {
                ID = Level.ID,
                Name = Level.Name,
                Description = Level.Description,
                RequiredAttendances = Level.RequiredAttendances,
                IsArchived = Level.IsArchived,
                ArchiveReason = Level.ArchiveReason,
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
