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
                        Attendee = new AttendeeViewModel(), // TODO
                        ExemptionID = 1,
                        Exemption = new ExemptionViewModel(), // TODO
                        LevelID = 1,
                        Level = new LevelViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 2,
                        Date = DateTime.Now,
                        ShortDescription = "Doloremque laudantium",
                        LongDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        ClubID = 1,
                        Club = new ClubViewModel(), // TODO
                        LevelID = 1,
                        Level = new LevelViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 3,
                        Date = DateTime.Now,
                        ShortDescription = "Error sit voluptatem accusantium",
                        LongDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                        ReservationID = 1,
                        Reservation = new ReservationViewModel(), // TODO
                        DinnerID = 1,
                        Dinner = new DinnerViewModel(), // TODO
                        RestaurantID = 1,
                        Restaurant = new RestaurantViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 4,
                        Date = DateTime.Now,
                        ShortDescription = "Unde omnis iste natus",
                        LongDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        ExemptionID = 1,
                        Exemption = new ExemptionViewModel(), // TODO
                        RestaurantID = 1,
                        Restaurant = new RestaurantViewModel(), // TODO
                        LevelID = 1,
                        Level = new LevelViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 6,
                        Date = DateTime.Now,
                        ShortDescription = "Sed ut perspiciatis",
                        LongDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        AttendeeID = 1,
                        Attendee = new AttendeeViewModel(), // TODO
                        ClubID = 1,
                        Club = new ClubViewModel(), // TODO
                        DinnerID = 1,
                        Dinner = new DinnerViewModel(), // TODO
                        ExemptionID = 1,
                        Exemption = new ExemptionViewModel(), // TODO
                        KotCID = 1,
                        KotC = new KotCViewModel(), // TODO
                        LevelID = 1,
                        Level = new LevelViewModel(), // TODO
                        MemberID = 1,
                        Member = new MemberViewModel(), // TODO
                        ReservationID = 1,
                        Reservation = new ReservationViewModel(), // TODO
                        RestaurantID = 1,
                        Restaurant = new RestaurantViewModel(), // TODO
                        RotYYear = 1,
                        RotY = new RotYViewModel(), // TODO
                        ViolationID = 1,
                        Violation = new ViolationViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 7,
                        Date = DateTime.Now,
                        ShortDescription = "Totam rem aperiam",
                        LongDescription = "At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.",
                        ReservationID = 1,
                        Reservation = new ReservationViewModel(), // TODO
                        ExemptionID = 1,
                        Exemption = new ExemptionViewModel(), // TODO
                        LevelID = 1,
                        Level = new LevelViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 8,
                        Date = DateTime.Now,
                        ShortDescription = "Doloremque laudantium",
                        LongDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        KotCID = 1,
                        KotC = new KotCViewModel(), // TODO
                        MemberID = 1,
                        Member = new MemberViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 9,
                        Date = DateTime.Now,
                        ShortDescription = "Error sit voluptatem accusantium",
                        LongDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                        ExemptionID = 1,
                        Exemption = new ExemptionViewModel(), // TODO
                        ReservationID = 1,
                        Reservation = new ReservationViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 10,
                        Date = DateTime.Now,
                        ShortDescription = "Unde omnis iste natus",
                        LongDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        ReservationID = 1,
                        Reservation = new ReservationViewModel(), // TODO
                        RestaurantID = 1,
                        Restaurant = new RestaurantViewModel(), // TODO
                        ClubID = 1,
                        Club = new ClubViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 11,
                        Date = DateTime.Now,
                        ShortDescription = "Sed ut perspiciatis",
                        LongDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        ReservationID = 1,
                        Reservation = new ReservationViewModel(), // TODO
                        RotYYear = 1,
                        RotY = new RotYViewModel(), // TODO
                        KotCID = 1,
                        KotC = new KotCViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 12,
                        Date = DateTime.Now,
                        ShortDescription = "Totam rem aperiam",
                        LongDescription = "At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.",
                        ViolationID = 1,
                        Violation = new ViolationViewModel(), // TODO
                        AttendeeID = 1,
                        Attendee = new AttendeeViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 13,
                        Date = DateTime.Now,
                        ShortDescription = "Doloremque laudantium",
                        LongDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        AttendeeID = 1,
                        Attendee = new AttendeeViewModel(), // TODO
                        RotYYear = 1,
                        RotY = new RotYViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 14,
                        Date = DateTime.Now,
                        ShortDescription = "Error sit voluptatem accusantium",
                        LongDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                        ClubID = 1,
                        Club = new ClubViewModel(), // TODO
                        ReservationID = 1,
                        Reservation = new ReservationViewModel(), // TODO
                        KotCID = 1,
                        KotC = new KotCViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 15,
                        Date = DateTime.Now,
                        ShortDescription = "Unde omnis iste natus",
                        LongDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        DinnerID = 1,
                        Dinner = new DinnerViewModel(), // TODO
                        KotCID = 1,
                        KotC = new KotCViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 16,
                        Date = DateTime.Now,
                        ShortDescription = "Sed ut perspiciatis",
                        LongDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        ExemptionID = 1,
                        Exemption = new ExemptionViewModel(), // TODO
                        RotYYear = 1,
                        RotY = new RotYViewModel(), // TODO
                        ReservationID = 1,
                        Reservation = new ReservationViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 17,
                        Date = DateTime.Now,
                        ShortDescription = "Totam rem aperiam",
                        LongDescription = "At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.",
                        LevelID = 1,
                        Level = new LevelViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 18,
                        Date = DateTime.Now,
                        ShortDescription = "Doloremque laudantium",
                        LongDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        KotCID = 1,
                        KotC = new KotCViewModel(), // TODO
                        ReservationID = 1,
                        Reservation = new ReservationViewModel(), // TODO
                        RestaurantID = 1,
                        Restaurant = new RestaurantViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 19,
                        Date = DateTime.Now,
                        ShortDescription = "Error sit voluptatem accusantium",
                        LongDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                        RestaurantID = 1,
                        Restaurant = new RestaurantViewModel(), // TODO
                        ClubID = 1,
                        Club = new ClubViewModel(), // TODO
                    },
                    new()
                    {
                        ID = 20,
                        Date = DateTime.Now,
                        ShortDescription = "Unde omnis iste natus",
                        LongDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        RotYYear = 1,
                        RotY = new RotYViewModel(), // TODO
                        AttendeeID = 1,
                        Attendee = new AttendeeViewModel(), // TODO
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
            NotificationItems.BatchBegin();
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
                }
            }
            NotificationItems.BatchCommit();
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
