using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
namespace acm_mobile_app.Views;

[QueryProperty(nameof(Dinner), "Dinner")]
public partial class EditDinner : ContentPage
{
    private readonly Dinner _dinner = new();

    public EditDinner()
	{
		InitializeComponent();
        BindingContext = Dinner;
    }

    public Dinner Dinner
    {
        get => _dinner;
        set
        {
            _dinner.ID = value.ID;
            _dinner.ReservationID = value.ReservationID;
            _dinner.Reservation = value.Reservation;
            _dinner.CostPerPerson = value.CostPerPerson;
            _dinner.NumBeersConsumed = value.NumBeersConsumed;
            _dinner.KotC = value.KotC;
        }
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        //if (string.IsNullOrWhiteSpace(FoundingFather.Text))
        //{
        //    var toast = Toast.Make("A founding father is required", ToastDuration.Short, 14);
        //    await toast.Show();
        //    return;
        //}

        //if (string.IsNullOrWhiteSpace(MemberName.Text))
        //{
        //    var toast = Toast.Make("A member is required", ToastDuration.Short, 14);
        //    await toast.Show();
        //    return;
        //}

        //if (string.IsNullOrWhiteSpace(ShortReason.Text))
        //{
        //    var toast = Toast.Make("A short reason is required", ToastDuration.Short, 14);
        //    await toast.Show();
        //    return;
        //}

        if (await EditExistingDinnerAsync())
            await Shell.Current.GoToAsync("//manage_dinners");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(async () => { await LoadDinnerData(); });
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async Task LoadDinnerData()
    {
        try
        {
            if (LoadingIndicator.IsRunning || _dinner.ID == null)
                return;

            LoadingIndicator.IsRunning = true;

            await Task.Delay(150);
            var dinnerModel = await AcmService.GetDinnerAsync((int)_dinner.ID);

            await Task.Delay(150);
            var x = Dinner.FromModel(dinnerModel);
            if (x != null)
            {
                Dinner = x;

                //if (_foundingFather == null && x.FoundingFather != null)
                //    _foundingFather = new Member() { ID = x.FoundingFather.ID, Name = x.FoundingFather.Name };

                //if (_member == null && x.Member != null)
                //    _member = new Member() { ID = x.Member.ID, Name = x.Member.Name };

                //CopyLocalStateToGui();

                var reservationModel = await AcmService.GetReservationAsync(x.ReservationID);
                Dinner.Reservation = Reservation.FromModel(reservationModel);

                // TODO:
                //   kotC
                //   roty
                //   violations
                //   attendees
                //   exemptions
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

    private async Task<bool> EditExistingDinnerAsync()
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
            await AcmService.UpdateDinnerAsync(new acm_models.Dinner
            {
                ID = Dinner.ID,
                ReservationID = Dinner.ReservationID,
                CostPerPerson = Dinner.CostPerPerson,
                NumBeersConsumed = Dinner.NumBeersConsumed,
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