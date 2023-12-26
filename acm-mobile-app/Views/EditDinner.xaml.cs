using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
namespace acm_mobile_app.Views;

[QueryProperty(nameof(Dinner), "Dinner")]
public partial class EditDinner : ContentPage
{
    private readonly Dinner _dinner = new();
    private readonly RotYStats _rotYStats = new();
    private List<Attendee> _attendees = [];

    public EditDinner()
	{
		InitializeComponent();
        BindingContext = this;
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
            OnPropertyChanged(nameof(Dinner));
            OnPropertyChanged();
        }
    }

    public RotYStats RotYStats
    {
        get => _rotYStats;
        set
        {
            _rotYStats.RestaurantID = value.RestaurantID;
            _rotYStats.IsCurrentRotY = value.IsCurrentRotY;
            _rotYStats.CurrentYear = value.CurrentYear;
            _rotYStats.FormerYears = value.FormerYears;
            OnPropertyChanged(nameof(RotYStats));
            OnPropertyChanged();
        }
    }

    public List<Attendee> Attendees
    {
        get => _attendees;
        set
        {
            _attendees = [];
            foreach (var item in value)
                _attendees.Add(item);
            OnPropertyChanged(nameof(Attendees));
            OnPropertyChanged();
        }
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        // TODO: add valiation rules
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
            await Shell.Current.GoToAsync("//home");
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void DinnerOrganiser_Clicked(object sender, EventArgs e)
    {
        // TODO
    }

    private void Restaurant_Clicked(object sender, EventArgs e)
    {
        // TODO
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

            var dinnerModel = await AcmService.GetDinnerAsync((int)_dinner.ID);

            var dinner = Dinner.FromModel(dinnerModel);
            if (dinner != null)
            {
                Dinner = dinner;

                var reservationModel = await AcmService.GetReservationAsync(dinner.ReservationID);
                if (reservationModel != null)
                    Dinner.Reservation = Reservation.FromModel(reservationModel);

                if (Dinner.Reservation != null)
                {
                    var restaurantModel = await AcmService.GetRestaurantAsync(Dinner.Reservation.RestaurantID);
                    if (restaurantModel != null)
                        Dinner.Reservation.Restaurant = Restaurant.FromModel(restaurantModel);

                    var rotyStatsModel = await AcmService.GetRestaurantRotYStatsAsync(Dinner.Reservation.RestaurantID);
                    if (rotyStatsModel != null)
                    {
                        var x = RotYStats.FromModel(rotyStatsModel);
                        if (x != null)
                            RotYStats = x;
                    }
                }

                // TODO: get the club ID from somewhere
                var dinnerAttendeeStats = await AcmService.ListDinnerAttendeeStatsAsync(1, (int)_dinner.ID);
                if (dinnerAttendeeStats != null)
                {
                    List<Attendee> attendees = [];
                    foreach (var m in dinnerAttendeeStats)
                    {
                        var attendee = Attendee.FromModel(m);
                        if (attendee != null)
                            attendees.Add(attendee);
                    }

                    Attendees = attendees;
                }

                // TODO:
                //violations that occurred
                //exemptions that were applied
                //inductees and their sponsor
                //members that went up a level

                OnPropertyChanged(nameof(Dinner));
                OnPropertyChanged(nameof(RotYStats));
                OnPropertyChanged(nameof(Attendees));
                OnPropertyChanged();
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
            // TODO: what other values can be changed by the user?

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