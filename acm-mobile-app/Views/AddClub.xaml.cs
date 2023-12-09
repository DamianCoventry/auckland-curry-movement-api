using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

[QueryProperty(nameof(SelectedMembers), "SelectedMembers")]
public partial class AddClub : ContentPage
{
    private List<SelectedMember> _selectedMembers = [];

    public AddClub()
	{
		InitializeComponent();
		BindingContext = this;
	}

    public ObservableCollection<Member> FoundingFathers { get; set; } = [];

    public bool AreFoundingFatherSelected { get { return FoundingFathers.Count > 0; } }
    public bool AreNoFoundingFatherSelected { get { return FoundingFathers.Count == 0; } }

    public List<SelectedMember> SelectedMembers
    {
        get => _selectedMembers;
        set
        {
            if (value == null)
                return;
            _selectedMembers = value;
            FoundingFathers.Clear();
            foreach (var member in _selectedMembers)
            {
                if (member.IsSelected)
                    FoundingFathers.Add(member.Member);
            }
            AddFF.IsVisible = AreNoFoundingFatherSelected;
            EditFF.IsVisible = AreFoundingFatherSelected;
        }
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ClubNameEntry.Text))
        {
            var toast = Toast.Make("A name for the club is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (FoundingFathers.Count == 0)
        {
            var toast = Toast.Make("At least one founding father for the club is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (await AddNewClubAsync())
            await Shell.Current.GoToAsync("//manage_clubs");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    public async void OnClickSelectMembers(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("select_members", true);
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async Task<bool> AddNewClubAsync()
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
            await AcmService.AddClubAsync(new acm_models.Club
            {
                ID = null,
                Name = ClubNameEntry.Text,
                IsArchived = false,
                ArchiveReason = null,
                Members = Utils.MemberUtils.ToModelsMemberList(FoundingFathers)
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
