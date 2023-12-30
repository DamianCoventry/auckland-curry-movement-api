using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.ViewModels;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Reservation), "Reservation")]
    public partial class EditReservationPage : ContentPage
    {
        private readonly ReservationViewModel _reservation = new();
        private MemberViewModel? _organiserSelection = null;
        private RestaurantViewModel? _restaurantSelection = null;

        public EditReservationPage()
        {
            InitializeComponent();
            BindingContext = Reservation;
        }

        public ReservationViewModel Reservation
        {
            get => _reservation;
            set
            {
                _reservation.ID = value.ID;
                _reservation.Year = value.Year;
                _reservation.Month = value.Month;
                _reservation.ExactDateTime = value.ExactDateTime;
                _reservation.NegotiatedBeerPrice = value.NegotiatedBeerPrice;
                _reservation.NegotiatedBeerDiscount = value.NegotiatedBeerDiscount;
                _reservation.IsAmnesty = value.IsAmnesty;

                if (_restaurantSelection != null && _restaurantSelection.ID != null)
                {
                    _reservation.RestaurantID = (int)_restaurantSelection.ID;
                    _reservation.Restaurant = new()
                    {
                        ID = _restaurantSelection.ID,
                        Name = _restaurantSelection.Name,
                        StreetAddress = _restaurantSelection.StreetAddress,
                        Suburb = _restaurantSelection.Suburb,
                        PhoneNumber = _restaurantSelection.PhoneNumber,
                    };
                }
                else
                {
                    _reservation.RestaurantID = value.RestaurantID;
                    _reservation.Restaurant = value.Restaurant;
                }

                if (_organiserSelection != null && _organiserSelection.ID != null)
                {
                    _reservation.OrganiserID = (int)_organiserSelection.ID;
                    _reservation.Organiser = new()
                    {
                        ID = _organiserSelection.ID,
                        Name = _organiserSelection.Name,
                    };
                }
                else
                {
                    _reservation.OrganiserID = value.OrganiserID;
                    _reservation.Organiser = value.Organiser;
                }
            }
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OrganiserEntry.Text))
            {
                var toast = Toast.Make("An restaurant for the reservation is required");
                await toast.Show();
                return;
            }

            if (string.IsNullOrWhiteSpace(RestaurantEntry.Text))
            {
                var toast = Toast.Make("A restaurant for the reservation is required");
                await toast.Show();
                return;
            }

            if (IsBeerPriceNegotiatedCheckBox.IsChecked)
            {
                bool priceIsEmpty = string.IsNullOrWhiteSpace(NegotiatedBeerPriceEntry.Text);
                bool discountIsEmpty = string.IsNullOrWhiteSpace(NegotiatedBeerDiscountEntry.Text);

                if (priceIsEmpty && discountIsEmpty)
                {
                    var toast = Toast.Make("Either a beer price or a beer discount is required.");
                    await toast.Show();
                    return;
                }
            }

            OrganiserEntry.IsEnabled = false;
            RestaurantEntry.IsEnabled = false;
            ChooseRestaurant.IsEnabled = false;
            AmnestyCheckBox.IsEnabled = false;
            ExactDateTimeDatePicker.IsEnabled = false;
            IsBeerPriceNegotiatedCheckBox.IsEnabled = false;
            NegotiatedBeerPriceEntry.IsEnabled = false;
            NegotiatedBeerDiscountEntry.IsEnabled = false;
            AcceptChanges.IsEnabled = false;
            DiscardChanges.IsEnabled = false;
            SavingIndicator.IsRunning = true;
            SavingIndicator.IsVisible = true;
            await Task.Delay(1500); // Fake saving
            await Shell.Current.GoToAsync("..");
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }

        private async void ChooseMember_Clicked(object sender, EventArgs e)
        {
            MemberViewModel? copy = null;
            _organiserSelection = null;

            if (_reservation.Organiser != null)
            {
                copy = new()
                {
                    ID = _reservation.Organiser.ID,
                    Name = _reservation.Organiser.Name,
                    IsArchived = _reservation.Organiser.IsArchived,
                    ArchiveReason = _reservation.Organiser.ArchiveReason,
                };
            }

            SelectSingleMemberPage page = new()
            {
                SelectedMember = copy,
                AcceptFunction = OrganiserSelection_Accepted,
            };

            await Navigation.PushAsync(page, true);
        }

        private void OrganiserSelection_Accepted(MemberViewModel selectedMember)
        {
            _organiserSelection = new()
            {
                ID = selectedMember.ID,
                Name = selectedMember.Name,
                IsArchived = selectedMember.IsArchived,
                ArchiveReason = selectedMember.ArchiveReason,
            };
        }

        private async void ChooseRestaurant_Clicked(object sender, EventArgs e)
        {
            RestaurantViewModel? copy = null;
            _restaurantSelection = null;

            if (_reservation.Restaurant != null)
            {
                copy = new()
                {
                    ID = _reservation.Restaurant.ID,
                    Name = _reservation.Restaurant.Name,
                    StreetAddress = _reservation.Restaurant.StreetAddress,
                    Suburb = _reservation.Restaurant.Suburb,
                    PhoneNumber = _reservation.Restaurant.PhoneNumber,
                };
            }

            SelectSingleRestaurantPage page = new()
            {
                SelectedRestaurant = copy,
                AcceptFunction = RestaurantSelection_Accepted,
            };

            await Navigation.PushAsync(page, true);
        }

        private void RestaurantSelection_Accepted(RestaurantViewModel selectedRestaurant)
        {
            _restaurantSelection = new()
            {
                ID = selectedRestaurant.ID,
                Name = selectedRestaurant.Name,
                StreetAddress = selectedRestaurant.StreetAddress,
                Suburb = selectedRestaurant.Suburb,
                PhoneNumber = selectedRestaurant.PhoneNumber,
            };
        }
    }
}
