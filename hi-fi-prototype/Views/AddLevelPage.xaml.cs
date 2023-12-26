using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;
namespace hi_fi_prototype.Views;

public partial class AddLevelPage : ContentPage
{
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

        LevelName.IsEnabled = false;
        LevelDescription.IsEnabled = false;
        LevelAttendances.IsEnabled = false;
        AcceptChanges.IsEnabled = false;
        DiscardChanges.IsEnabled = false;
        SavingIndicator.IsRunning = true;
        SavingIndicator.IsVisible = true;
        await Task.Delay(1500); // Fake saving
        await Shell.Current.GoToAsync("..");
    }

    private async void ManageNotifications_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("manage_notifications");
    }
}
