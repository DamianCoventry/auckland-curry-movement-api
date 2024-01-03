using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
namespace hi_fi_prototype.Views;

public partial class AddLevelPage : ContentPage
{
    private const int MIN_REFRESH_TIME_MS = 500;
    private readonly LevelViewModel _level = new();

    public AddLevelPage()
    {
        InitializeComponent();
        BindingContext = Level;
    }

    public LevelViewModel Level
    {
        get => _level;
        set
        {
            _level.ID = value.ID;
            _level.Name = value.Name;
            _level.Description = value.Description;
            _level.RequiredAttendances = value.RequiredAttendances;
            _level.IsArchived = value.IsArchived;
            _level.ArchiveReason = value.ArchiveReason;
        }
    }

    private async void DiscardChanges_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void AcceptChanges_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_level.Name))
        {
            var toast = Toast.Make("A name for the level is required");
            await toast.Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(_level.Description))
        {
            var toast = Toast.Make("A description for the level is required");
            await toast.Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(LevelAttendances.Text))
        {
            var toast = Toast.Make("The number of required curries for the level is required");
            await toast.Show();
            return;
        }

        if (_level.RequiredAttendances < 0 || _level.RequiredAttendances > 1000)
        {
            var toast = Toast.Make("The number of required curries must be >= 0 and <= 1000");
            await toast.Show();
            return;
        }

        MainThread.BeginInvokeOnMainThread(async () => { await SendAddLevelMessage(); });
    }

    private void ShowMessageSendingGui(bool show = true)
    {
        LevelName.IsEnabled = !show;
        LevelDescription.IsEnabled = !show;
        LevelAttendances.IsEnabled = !show;
        AcceptChanges.IsEnabled = !show;
        DiscardChanges.IsEnabled = !show;
        SavingIndicator.IsRunning = show;
        SavingIndicator.IsVisible = show;
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async Task SendAddLevelMessage()
    {
        bool saved = false;
        try
        {
            if (SavingIndicator.IsRunning)
                return;

            ShowMessageSendingGui();

            var startTime = DateTime.Now;

            var reservation = await AcmService.AddLevelAsync(new acm_models.Level()
            {
                ID = Level.ID,
                RequiredAttendances = Level.RequiredAttendances,
                Name = Level.Name,
                Description = Level.Description,
                IsArchived = Level.IsArchived,
                ArchiveReason = Level.ArchiveReason,
            });

            var elapsed = DateTime.Now - startTime;
            if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);

            saved = reservation.ID != null;
            if (!saved)
            {
                var toast = Toast.Make("Unable to add these data as a new level");
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

    private async void ManageNotifications_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("manage_notifications");
    }
}
