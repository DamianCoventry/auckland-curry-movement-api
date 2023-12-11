using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
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
    private int _page = 0;
    private int _totalPages = 0;

    private readonly List<SelectedMember> _selectedMembers = [];

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
            foreach (var x in value)
                AddOrUpdate(x);

            CopySelectedMembersToFoundingFathers();
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
        List<SelectedMember> copy = [];
        foreach (var x in SelectedMembers)
            copy.Add(new SelectedMember() { IsSelected = x.IsSelected, Member = x.Member });

        Dictionary<string, object> parameters = new() { { "ClubID", ClubID }, { "SelectedMembers", copy } };
        await Shell.Current.GoToAsync("select_0_1_or_more_members", true, parameters);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ClubNameEntry.Text = ClubName;
        MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
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

    private async Task RefreshListData()
    {
        try
        {
            if (LoadingIndicator.IsRunning)
                return;

            LoadingIndicator.IsRunning = true;

            await Task.Delay(150);
            var foundingFathers = await AcmService.ListClubFoundingFathersAsync(ClubID, _page * PAGE_SIZE, PAGE_SIZE);
            _totalPages = foundingFathers.PageItems == null ? 0 : foundingFathers.TotalPages;

            ShowMoreButton.IsVisible = _totalPages > 1 && _page < (_totalPages - 1);

            await Task.Delay(150);
            if (foundingFathers.PageItems != null)
            {
                foreach (var model in foundingFathers.PageItems)
                {
                    var x = Member.FromModel(model);
                    if (x != null)
                        AddIfAbsent(new SelectedMember() { IsSelected = true, Member = x });
                }

                CopySelectedMembersToFoundingFathers();
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

    private void AddIfAbsent(SelectedMember selectedMember)
    {
        var x = _selectedMembers.Where(x => x.Member.ID == selectedMember.Member.ID).FirstOrDefault();
        if (x == null)
            _selectedMembers.Add(selectedMember);
    }

    private void AddOrUpdate(SelectedMember selectedMember)
    {
        var x = _selectedMembers.Where(x => x.Member.ID == selectedMember.Member.ID).FirstOrDefault();
        if (x == null)
            _selectedMembers.Add(selectedMember);
        else
            x.IsSelected = selectedMember.IsSelected;
    }

    private void CopySelectedMembersToFoundingFathers()
    {
        FoundingFathers.Clear();
        foreach (var x in _selectedMembers)
        {
            if (x.IsSelected)
                FoundingFathers.Add(x.Member);
        }
    }

    private void ShowMoreButton_Clicked(object sender, EventArgs e)
    {
        if (_page < _totalPages - 1)
        {
            ++_page;
            MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
        }
    }
}