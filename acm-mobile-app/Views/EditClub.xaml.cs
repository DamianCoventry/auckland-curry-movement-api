using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.VisualBasic;
using System.Collections;
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
        get
        {
            _selectedMembers = [];
            foreach (var foundingFather in FoundingFathers)
                _selectedMembers.Add(new SelectedMember() { IsSelected = true, Member = foundingFather });
            return _selectedMembers;
        }
        set
        {
            if (value == null)
                return;
            _selectedMembers = value;
            FoundingFathers.Clear();
            SynchroniseFoundingFatherContainer();
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
        Dictionary<string, object> parameters = new() { { "ClubID", ClubID }, { "SelectedMembers", SelectedMembers } };
        await Shell.Current.GoToAsync("select_members", true, parameters);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ClubNameEntry.Text = ClubName;
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
            var ffs = await AcmService.UpdateClubAsync(new acm_models.Club
            {
                ID = ClubID,
                Name = ClubNameEntry.Text,
                IsArchived = false,
                ArchiveReason = null,
                Members = Utils.MemberUtils.ToModelsMemberList(FoundingFathers),
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
                foreach (var model in foundingFathers.PageItems)
                {
                    var x = Member.FromModel(model);
                    if (x != null)
                        FoundingFathers.Add(x);
                }
            }

            SynchroniseFoundingFatherContainer();
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

    private void SynchroniseFoundingFatherContainer()
    {
        foreach (var member in _selectedMembers)
        {
            var existingFoundingFather = FoundingFathers.FirstOrDefault(x => x.ID == member.Member.ID);
            if (member.IsSelected && existingFoundingFather == null)
                FoundingFathers.Add(member.Member);
            else if (!member.IsSelected && existingFoundingFather != null)
                FoundingFathers.Remove(existingFoundingFather);
        }
    }
}