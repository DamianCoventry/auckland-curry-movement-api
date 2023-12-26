using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;
namespace hi_fi_prototype.Views;

public partial class AddClubPage : ContentPage
{
    private readonly ClubViewModel _club = new();

    public AddClubPage()
    {
        InitializeComponent();
        BindingContext = Club;
    }

    public ClubViewModel Club
    {
        get => _club;
        set
        {
            _club.ID = value.ID;
            _club.Name = value.Name;
            _club.IsArchived = value.IsArchived;
            _club.ArchiveReason = value.ArchiveReason;

            ObservableCollection<MemberViewModel> copy = [];
            foreach (var i in value.FoundingFathers)
                copy.Add(new MemberViewModel() { ID = i.ID, Name = i.Name });
            _club.FoundingFathers = copy;
        }
    }

    private async void SelectFoundingFathers_Clicked(object sender, EventArgs e)
    {
        string? name = await DisplayPromptAsync("Add Founding Father",
            "How will the brotherhood refer to this gentleman?", "OK",
            "Cancel", "Enter a name", 100);
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
        {
            Club.FoundingFathers.Add(new MemberViewModel()
            {
                Name = name,
            });
        }
    }

    private async void DiscardChanges_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void AcceptChanges_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_club.Name))
        {
            var toast = Toast.Make("A name for the club is required");
            await toast.Show();
            return;
        }

        if (_club.FoundingFathers.Count == 0)
        {
            var toast = Toast.Make("At fewest 1 founding father is required");
            await toast.Show();
            return;
        }

        ClubName.IsEnabled = false;
        SelectFoundingFathers.IsEnabled = false;
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
