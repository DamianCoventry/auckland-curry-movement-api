using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;

namespace hi_fi_prototype.Views
{
    [QueryProperty(nameof(Reservation), "Reservation")]
    public partial class EditReservationPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private readonly ReservationViewModel _reservation = new();
        private MembershipViewModel? _organiserSelection = null;
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

                if (_organiserSelection != null && _organiserSelection.Member != null)
                {
                    _reservation.OrganiserID = _organiserSelection.MemberID;
                    _reservation.Organiser = new()
                    {
                        Member = new MemberViewModel()
                        {
                            ID = _organiserSelection.Member.ID,
                            Name = _organiserSelection.Member.Name,
                        }
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
                var toast = Toast.Make("An organiser for the reservation is required");
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

            MainThread.BeginInvokeOnMainThread(async () => { await SendSaveReservationMessage(); });
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }

        private async void ChooseMember_Clicked(object sender, EventArgs e)
        {
            MembershipViewModel? copy = null;
            _organiserSelection = null;

            if (_reservation.Organiser != null)
            {
                var m = _reservation.Organiser.Member;
                if (m != null)
                {
                    copy = new()
                    {
                        Member = new MemberViewModel()
                        {
                            ID = m.ID, Name = m.Name, IsArchived = m.IsArchived, ArchiveReason = m.ArchiveReason,
                        },
                    };
                }
            }

            SelectSingleMemberPage page = new()
            {
                SelectedMember = copy,
                AcceptFunction = OrganiserSelection_Accepted,
            };

            await Navigation.PushAsync(page, true);
        }

        private void OrganiserSelection_Accepted(MembershipViewModel selectedMember)
        {
            var m = selectedMember.Member;
            if (m == null)
                return;

            _organiserSelection = new()
            {
                ClubID = selectedMember.ClubID,
                Member = new MemberViewModel()
                {
                    ID = m.ID, Name = m.Name, IsArchived = m.IsArchived, ArchiveReason = m.ArchiveReason,
                }
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

        private void ShowMessageSendingGui(bool show = true)
        {
            OrganiserEntry.IsEnabled = !show;
            RestaurantEntry.IsEnabled = !show;
            ChooseRestaurant.IsEnabled = !show;
            AmnestyCheckBox.IsEnabled = !show;
            ExactDateTimeDatePicker.IsEnabled = !show;
            IsBeerPriceNegotiatedCheckBox.IsEnabled = !show;
            NegotiatedBeerPriceEntry.IsEnabled = !show;
            NegotiatedBeerDiscountEntry.IsEnabled = !show;
            AcceptChanges.IsEnabled = !show;
            DiscardChanges.IsEnabled = !show;
            SavingIndicator.IsRunning = show;
            SavingIndicator.IsVisible = show;
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task SendSaveReservationMessage()
        {
            bool saved = false;
            try
            {
                if (SavingIndicator.IsRunning)
                    return;

                ShowMessageSendingGui();

                var startTime = DateTime.Now;

                saved = await AcmService.UpdateReservationAsync(new acm_models.Reservation()
                {
                    ID = Reservation.ID,
                    OrganiserID = Reservation.OrganiserID,
                    RestaurantID = Reservation.RestaurantID,
                    ExactDateTime = Reservation.ExactDateTime,
                    Year = Reservation.ExactDateTime.Year,
                    Month = Reservation.ExactDateTime.Month,
                    NegotiatedBeerPrice = Reservation.NegotiatedBeerPrice,
                    NegotiatedBeerDiscount = Reservation.NegotiatedBeerDiscount,
                    IsAmnesty = Reservation.IsAmnesty,
                });

                var elapsed = DateTime.Now - startTime;
                if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                    await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);

                if (!saved)
                {
                    var toast = Toast.Make("Unable to add these data as a new reservation");
                    await toast.Show();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                ShowMessageSendingGui(false);
            }

            if (saved)
                await Shell.Current.GoToAsync("..");
        }
    }
}
