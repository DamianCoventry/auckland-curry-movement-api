using acm_mobile_app.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

[QueryProperty(nameof(ClubID), "ClubID")]
[QueryProperty(nameof(ClubName), "ClubName")]
[QueryProperty(nameof(SelectedMembers), "SelectedMembers")]
public partial class EditClub : ContentPage
{
    private const int PAGE_SIZE = 10;
    private List<SelectedMember> _selectedMembers = [];

    public EditClub()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public int ClubID { get; set; }
    public string ClubName { get; set; } = string.Empty;

    public ObservableCollection<Member> FoundingFathers { get; set; } = [];

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

        if (await EditExistingClubAsync())
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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(async () => { await RefreshFoundingFathers(); });
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async Task<bool> EditExistingClubAsync()
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
            var ffs = await AcmService.UpdateClubAsync(new Viewviewmodels:Club
            {
                ID = ClubID,
                Name = ClubNameEntry.Text,
                IsArchived = false,
                ArchiveReason = null,
                Members = new List<Member>(FoundingFathers)
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

    private async Task RefreshFoundingFathers()
    {
        try
        {
            if (LoadingIndicator.IsRunning)
                return;

            LoadingIndicator.IsRunning = true;

            await Task.Delay(150);
            var foundingFathers = await AcmService.ListClubFoundingFathersAsync(ClubID, 0, PAGE_SIZE); // TODO: request different pages

            await Task.Delay(150);
            FoundingFathers.Clear();

            if (foundingFathers.PageItems != null)
            {
                foreach (var ff in foundingFathers.PageItems)
                    FoundingFathers.Add(ff);
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
}