using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;

namespace acm_mobile_app.Views;

public partial class ManageExemptions : ContentPage
{
    private const int PAGE_SIZE = 10;
    private int _page = 0;
    private int _totalPages = 0;
    private bool _isDeleting = false;

    public ManageExemptions()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public int CurrentPage { get { return _page + 1; } }

    public ObservableCollection<Exemption> Exemptions { get; set; } = [];

    public void OnClickRefreshData(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
    }

    public async void OnClickAdd(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("add_exemption");
    }

    public async void OnClickModify(object sender, EventArgs e)
    {
        if (ExemptionListView.SelectedItem == null)
        {
            var toast = Toast.Make("Select an exemption first", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (ExemptionListView.SelectedItem is Exemption exemption)
        {
            Exemption copy = new()
            {
                ID = exemption.ID,
                FoundingFatherID = exemption.FoundingFatherID,
                MemberID = exemption.MemberID,
                Date = exemption.Date,
                ShortReason = exemption.ShortReason,
                LongReason = exemption.LongReason,
                IsArchived = exemption.IsArchived,
                ArchiveReason = exemption.ArchiveReason,
            };

            Dictionary<string, object> parameters = new() { { "Exemption", copy } };
            await Shell.Current.GoToAsync("edit_exemption", true, parameters);
        }
    }

    public async void OnClickDelete(object sender, EventArgs e)
    {
        if (ExemptionListView.SelectedItem == null)
        {
            var toast = Toast.Make("Select an exemption first", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (await DisplayAlert("Delete Exemption", "Are you sure you want to delete the selected exemption?", "Yes", "No"))
        {
            MainThread.BeginInvokeOnMainThread(async () => { await DeleteSelectedItem(); });
        }
    }

    public void OnClickFirst(object sender, EventArgs e)
    {
        if (_page > 0)
        {
            _page = 0;
            MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
            CurrentPageNumber.Text = (1 + _page).ToString();
        }
    }

    public void OnClickPrevious(object sender, EventArgs e)
    {
        if (_page > 0)
        {
            --_page;
            MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
            CurrentPageNumber.Text = (1 + _page).ToString();
        }
    }

    public void OnClickNext(object sender, EventArgs e)
    {
        if (_page < _totalPages - 1)
        {
            ++_page;
            MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
            CurrentPageNumber.Text = (1 + _page).ToString();
        }
    }

    public void OnClickLast(object sender, EventArgs e)
    {
        if (_page < _totalPages - 1)
        {
            _page = _totalPages - 1;
            MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
            CurrentPageNumber.Text = (1 + _page).ToString();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(async () => { await RefreshListData(); });
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async Task RefreshListData()
    {
        try
        {
            if (IsRefreshingListData.IsRunning)
                return;

            IsRefreshingListData.IsRunning = true;
            await Task.Delay(150);

            var exemptions = await AcmService.ListExemptionsAsync(_page * PAGE_SIZE, PAGE_SIZE);
            _totalPages = exemptions.PageItems == null ? 0 : exemptions.TotalPages;

            await Task.Delay(150);

            ExemptionListView.SelectedItem = null;
            Exemptions.Clear();

            if (exemptions.PageItems != null)
            {
                foreach (var model in exemptions.PageItems)
                {
                    var x = Exemption.FromModel(model);
                    if (x != null)
                        Exemptions.Add(x);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Close");
        }
        finally
        {
            IsRefreshingListData.IsRunning = false;
        }
    }

    private async Task DeleteSelectedItem()
    {
        try
        {
            if (_isDeleting || ExemptionListView.SelectedItem == null)
                return;

            if (ExemptionListView.SelectedItem is not Exemption selectedExemption || selectedExemption.ID == null)
                return;

            _isDeleting = true;
            await Task.Delay(150);

            // TODO: don't actually delete the exemption, archive it.
            await AcmService.DeleteExemptionAsync((int)selectedExemption.ID);

            await RefreshListData();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Close");
        }
        finally
        {
            _isDeleting = false;
        }
    }
}
