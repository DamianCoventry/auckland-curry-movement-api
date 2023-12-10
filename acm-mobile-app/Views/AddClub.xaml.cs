using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

public partial class AddClub : ContentPage
{
    public AddClub()
	{
		InitializeComponent();
		BindingContext = this;
	}

    public ObservableCollection<Member> FoundingFathers { get; set; } = [];

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

    public async void AddFoundingFather_Clicked(object sender, EventArgs e)
    {
        string? name = await DisplayPromptAsync("Add Founding Father", "How will the brotherhood refer to this gentleman?", placeholder: "Enter a name", maxLength: 100);
        if (name != null)
        {
            name = name.Trim();
            if (string.IsNullOrEmpty(name))
            {
                var toast = Toast.Make("The name must not be empty or only white space", ToastDuration.Short, 14);
                await toast.Show();
            }
            else if (FoundingFathers.FirstOrDefault(x => string.Compare(x.Name, name, true) == 0) != null)
            {
                var toast = Toast.Make("That name is already in use", ToastDuration.Short, 14);
                await toast.Show();
            }
            else
            {
                FoundingFathers.Add(new Member() { Name = name });
                FoudingFathersAbsent.IsVisible = false;
                FoudingFathersPresent.IsVisible = true;
            }
        }
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
