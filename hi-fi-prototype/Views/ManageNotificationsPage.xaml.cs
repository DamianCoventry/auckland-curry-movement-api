using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageNotificationsPage : ContentPage
    {
        private ObservableCollection<NotificationViewModel> _notifications = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageNotificationsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int PageSize { get; set; } = 10;

        public ObservableCollection<NotificationViewModel> Notifications
        {
            get => _notifications;
            set
            {
                _notifications = [];
                foreach (var notification in value)
                {
                    var copy = new NotificationViewModel
                    {
                        ID = notification.ID,
                        Date = notification.Date,
                        ShortDescription = notification.ShortDescription,
                        LongDescription = notification.LongDescription,
                        Attendee = notification.Attendee,
                        AttendeeID = notification.AttendeeID,
                        Club = notification.Club,
                        ClubID = notification.ClubID,
                        Dinner = notification.Dinner,
                        DinnerID = notification.DinnerID,
                        Exemption = notification.Exemption,
                        ExemptionID = notification.ExemptionID,
                        KotC = notification.KotC,
                        KotCID = notification.KotCID,
                        Level = notification.Level,
                        LevelID = notification.LevelID,
                        Member = notification.Member,
                        MemberID = notification.MemberID,
                        Reservation = notification.Reservation,
                        ReservationID = notification.ReservationID,
                        Restaurant = notification.Restaurant,
                        RestaurantID = notification.RestaurantID,
                        RotY = notification.RotY,
                        RotYYear = notification.RotYYear,
                        Violation = notification.Violation,
                        ViolationID = notification.ViolationID,
                    };
                    _notifications.Add(copy);
                }
                OnPropertyChanged(nameof(Notifications));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
        }

        private async Task LoadData()
        {
            try
            {
                LoadingIndicator.IsRunning = true;
                NotificationItems.IsEnabled = false;

                await Task.Delay(750);

                var pretendRdbms = new ObservableCollection<NotificationViewModel>() {
                    new()
                    {
                        ID = 1,
                        Date = DateTime.Now,
                        ShortDescription = "Totam rem aperiam",
                        LongDescription = "At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.",
                        AttendeeID = 1,
                        Attendee = new acm_models.Attendee(),
                        ExemptionID = 1,
                        Exemption = new acm_models.Exemption(),
                        LevelID = 1,
                        Level = new acm_models.Level(),
                    },
                    new()
                    {
                        ID = 2,
                        Date = DateTime.Now,
                        ShortDescription = "Doloremque laudantium",
                        LongDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        ClubID = 1,
                        Club = new acm_models.Club(),
                        LevelID = 1,
                        Level = new acm_models.Level(),
                    },
                    new()
                    {
                        ID = 3,
                        Date = DateTime.Now,
                        ShortDescription = "Error sit voluptatem accusantium",
                        LongDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                        ReservationID = 1,
                        Reservation = new acm_models.Reservation(),
                        DinnerID = 1,
                        Dinner = new acm_models.Dinner(),
                        RestaurantID = 1,
                        Restaurant = new acm_models.Restaurant(),
                    },
                    new()
                    {
                        ID = 4,
                        Date = DateTime.Now,
                        ShortDescription = "Unde omnis iste natus",
                        LongDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        ExemptionID = 1,
                        Exemption = new acm_models.Exemption(),
                        RestaurantID = 1,
                        Restaurant = new acm_models.Restaurant(),
                        LevelID = 1,
                        Level = new acm_models.Level(),
                    },
                    new()
                    {
                        ID = 6,
                        Date = DateTime.Now,
                        ShortDescription = "Sed ut perspiciatis",
                        LongDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        RotYYear = 1,
                        RotY = new acm_models.RotY(),
                        KotCID = 1,
                        KotC = new acm_models.KotC(),
                        ClubID = 1,
                        Club = new acm_models.Club(),
                    },
                    new()
                    {
                        ID = 7,
                        Date = DateTime.Now,
                        ShortDescription = "Totam rem aperiam",
                        LongDescription = "At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.",
                        ReservationID = 1,
                        Reservation = new acm_models.Reservation(),
                        ExemptionID = 1,
                        Exemption = new acm_models.Exemption(),
                        LevelID = 1,
                        Level = new acm_models.Level(),
                    },
                    new()
                    {
                        ID = 8,
                        Date = DateTime.Now,
                        ShortDescription = "Doloremque laudantium",
                        LongDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        KotCID = 1,
                        KotC = new acm_models.KotC(),
                        MemberID = 1,
                        Member = new acm_models.Member(),
                    },
                    new()
                    {
                        ID = 9,
                        Date = DateTime.Now,
                        ShortDescription = "Error sit voluptatem accusantium",
                        LongDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                        ExemptionID = 1,
                        Exemption = new acm_models.Exemption(),
                        ReservationID = 1,
                        Reservation = new acm_models.Reservation(),
                    },
                    new()
                    {
                        ID = 10,
                        Date = DateTime.Now,
                        ShortDescription = "Unde omnis iste natus",
                        LongDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        ReservationID = 1,
                        Reservation = new acm_models.Reservation(),
                        RestaurantID = 1,
                        Restaurant = new acm_models.Restaurant(),
                        ClubID = 1,
                        Club = new acm_models.Club(),
                    },
                    new()
                    {
                        ID = 11,
                        Date = DateTime.Now,
                        ShortDescription = "Sed ut perspiciatis",
                        LongDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        ReservationID = 1,
                        Reservation = new acm_models.Reservation(),
                        RotYYear = 1,
                        RotY = new acm_models.RotY(),
                        KotCID = 1,
                        KotC = new acm_models.KotC(),
                    },
                    new()
                    {
                        ID = 12,
                        Date = DateTime.Now,
                        ShortDescription = "Totam rem aperiam",
                        LongDescription = "At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.",
                        ViolationID = 1,
                        Violation = new acm_models.Violation(),
                        AttendeeID = 1,
                        Attendee = new acm_models.Attendee(),
                    },
                    new()
                    {
                        ID = 13,
                        Date = DateTime.Now,
                        ShortDescription = "Doloremque laudantium",
                        LongDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        AttendeeID = 1,
                        Attendee = new acm_models.Attendee(),
                        RotYYear = 1,
                        RotY = new acm_models.RotY(),
                    },
                    new()
                    {
                        ID = 14,
                        Date = DateTime.Now,
                        ShortDescription = "Error sit voluptatem accusantium",
                        LongDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                        ClubID = 1,
                        Club = new acm_models.Club(),
                        ReservationID = 1,
                        Reservation = new acm_models.Reservation(),
                        KotCID = 1,
                        KotC = new acm_models.KotC(),
                    },
                    new()
                    {
                        ID = 15,
                        Date = DateTime.Now,
                        ShortDescription = "Unde omnis iste natus",
                        LongDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        DinnerID = 1,
                        Dinner = new acm_models.Dinner(),
                        KotCID = 1,
                        KotC = new acm_models.KotC(),
                    },
                    new()
                    {
                        ID = 16,
                        Date = DateTime.Now,
                        ShortDescription = "Sed ut perspiciatis",
                        LongDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        ExemptionID = 1,
                        Exemption = new acm_models.Exemption(),
                        RotYYear = 1,
                        RotY = new acm_models.RotY(),
                        ReservationID = 1,
                        Reservation = new acm_models.Reservation(),
                    },
                    new()
                    {
                        ID = 17,
                        Date = DateTime.Now,
                        ShortDescription = "Totam rem aperiam",
                        LongDescription = "At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.",
                        LevelID = 1,
                        Level = new acm_models.Level(),
                    },
                    new()
                    {
                        ID = 18,
                        Date = DateTime.Now,
                        ShortDescription = "Doloremque laudantium",
                        LongDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        KotCID = 1,
                        KotC = new acm_models.KotC(),
                        ReservationID = 1,
                        Reservation = new acm_models.Reservation(),
                        RestaurantID = 1,
                        Restaurant = new acm_models.Restaurant(),
                    },
                    new()
                    {
                        ID = 19,
                        Date = DateTime.Now,
                        ShortDescription = "Error sit voluptatem accusantium",
                        LongDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                        RestaurantID = 1,
                        Restaurant = new acm_models.Restaurant(),
                        ClubID = 1,
                        Club = new acm_models.Club(),
                    },
                    new()
                    {
                        ID = 20,
                        Date = DateTime.Now,
                        ShortDescription = "Unde omnis iste natus",
                        LongDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        RotYYear = 1,
                        RotY = new acm_models.RotY(),
                        AttendeeID = 1,
                        Attendee = new acm_models.Attendee(),
                    },
                };

                _totalPages = pretendRdbms.Count / PageSize;
                if (pretendRdbms.Count % PageSize > 0)
                    _totalPages++;
                _currentPage = Math.Min(Math.Max(0, _currentPage), _totalPages - 1);

                LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;

                MergePageIntoListView(pretendRdbms.Skip(_currentPage * PageSize).Take(PageSize).ToList());

                await Task.Delay(750);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                NotificationItems.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<NotificationViewModel> pageOfNotifications)
        {
            bool changed = false;
            foreach (var notification in pageOfNotifications)
            {
                var x = _notifications.FirstOrDefault(y => y.ID == notification.ID);
                if (x == null)
                {
                    _notifications.Add(new NotificationViewModel()
                    {
                        ID = notification.ID,
                        Date = notification.Date,
                        ShortDescription = notification.ShortDescription,
                        LongDescription = notification.LongDescription,
                        Attendee = notification.Attendee,
                        AttendeeID = notification.AttendeeID,
                        Club = notification.Club,
                        ClubID = notification.ClubID,
                        Dinner = notification.Dinner,
                        DinnerID = notification.DinnerID,
                        Exemption = notification.Exemption,
                        ExemptionID = notification.ExemptionID,
                        KotC = notification.KotC,
                        KotCID = notification.KotCID,
                        Level = notification.Level,
                        LevelID = notification.LevelID,
                        Member = notification.Member,
                        MemberID = notification.MemberID,
                        Reservation = notification.Reservation,
                        ReservationID = notification.ReservationID,
                        Restaurant = notification.Restaurant,
                        RestaurantID = notification.RestaurantID,
                        RotY = notification.RotY,
                        RotYYear = notification.RotYYear,
                        Violation = notification.Violation,
                        ViolationID = notification.ViolationID,
                    });
                    changed = true;
                }
            }
            if (changed)
                OnPropertyChanged(nameof(Notifications));
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
            }
        }
    }
}
