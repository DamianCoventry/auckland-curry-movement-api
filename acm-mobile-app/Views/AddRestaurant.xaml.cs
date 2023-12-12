using acm_mobile_app.Services;
using acm_mobile_app.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

public partial class AddRestaurant : ContentPage
{
    private readonly Restaurant _restaurant = new();

    public AddRestaurant()
    {
        InitializeComponent();
        BindingContext = _restaurant;
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

        if (await AddNewRestaurantAsync())
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

    private async Task<bool> AddNewRestaurantAsync()
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
            await AcmService.AddRestaurantAsync(new acm_models.Restaurant
            {
                Name = _restaurant.Name,
                StreetAddress = _restaurant.StreetAddress,
                Suburb = _restaurant.Suburb,
                PhoneNumber = _restaurant.PhoneNumber,
                IsArchived = _restaurant.IsArchived,
                ArchiveReason = _restaurant.ArchiveReason,
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
