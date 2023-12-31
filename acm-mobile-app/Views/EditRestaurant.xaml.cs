using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

[QueryProperty(nameof(Restaurant), "Restaurant")]
public partial class EditRestaurant : ContentPage
{
    private readonly Restaurant _restaurant = new();

    public EditRestaurant()
    {
        InitializeComponent();
        BindingContext = Restaurant;
    }

    public Restaurant Restaurant
    {
        get => _restaurant;
        set
        {
            _restaurant.ID = value.ID;
            _restaurant.Name = value.Name;
            _restaurant.StreetAddress = value.StreetAddress;
            _restaurant.Suburb = value.Suburb;
            _restaurant.PhoneNumber = value.PhoneNumber;
            _restaurant.IsArchived = value.IsArchived;
            _restaurant.ArchiveReason = value.ArchiveReason;
        }
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RestaurantName.Text))
        {
            var toast = Toast.Make("A name is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(Suburb.Text))
        {
            var toast = Toast.Make("A suburb is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        if (await EditExistingRestaurantAsync())
            await Shell.Current.GoToAsync("//manage_restaurants");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private static IAcmService AcmService
    {
        get { return ((AppShell)Shell.Current).AcmService; }
    }

    private async Task<bool> EditExistingRestaurantAsync()
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
            await AcmService.UpdateRestaurantAsync(new acm_models.Restaurant
            {
                ID = Restaurant.ID,
                Name = Restaurant.Name,
                StreetAddress = Restaurant.StreetAddress,
                Suburb = Restaurant.Suburb,
                PhoneNumber = Restaurant.PhoneNumber,
                IsArchived = Restaurant.IsArchived,
                ArchiveReason = Restaurant.ArchiveReason,
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
