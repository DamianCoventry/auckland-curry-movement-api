using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageNotificationsPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
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
                        Membership = notification.Membership,
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
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfNotifications(); });
        }

        private void RefreshNotificationsList_Clicked(object sender, EventArgs e)
        {
            Notifications = [];
            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfNotifications(); });
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task DownloadSinglePageOfNotifications()
        {
            try
            {
                if (LoadingIndicator.IsRunning)
                    return;

                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                NotificationItems.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;

                var pageOfData = await AcmService.ListNotificationsAsync(_currentPage * PageSize, PageSize);
                if (pageOfData != null && pageOfData.PageItems != null)
                {
                    _totalPages = pageOfData.TotalPages;
                    LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
                    MergePageIntoListView(pageOfData.PageItems);
                }

                var elapsed = DateTime.Now - startTime;
                if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                    await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                NotificationItems.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<acm_models.Notification> pageOfNotifications)
        {
            NotificationItems.BatchBegin();
            foreach (var notification in pageOfNotifications)
            {
                var existing = _notifications.FirstOrDefault(y => y.ID == notification.ID);
                if (existing == null)
                {
                    var vm = NotificationViewModel.FromModel(notification);
                    if (vm != null)
                        _notifications.Add(vm);
                }
                else
                {
                    existing.ID = notification.ID;
                    existing.Date = notification.Date;
                    existing.ShortDescription = notification.ShortDescription;
                    existing.LongDescription = notification.LongDescription;
                    existing.Attendee = AttendeeViewModel.FromModel(notification.Attendee);
                    existing.AttendeeID = notification.AttendeeID;
                    existing.Club = ClubViewModel.FromModel(notification.Club);
                    existing.ClubID = notification.ClubID;
                    existing.Dinner = DinnerViewModel.FromModel(notification.Dinner);
                    existing.DinnerID = notification.DinnerID;
                    existing.Exemption = ExemptionViewModel.FromModel(notification.Exemption);
                    existing.ExemptionID = notification.ExemptionID;
                    existing.KotC = KotCViewModel.FromModel(notification.KotC);
                    existing.KotCID = notification.KotCID;
                    existing.Level = LevelViewModel.FromModel(notification.Level);
                    existing.LevelID = notification.LevelID;
                    existing.Membership = MembershipViewModel.FromModel(notification.Membership);
                    existing.MemberID = notification.MemberID;
                    existing.Reservation = ReservationViewModel.FromModel(notification.Reservation);
                    existing.ReservationID = notification.ReservationID;
                    existing.Restaurant = RestaurantViewModel.FromModel(notification.Restaurant);
                    existing.RestaurantID = notification.RestaurantID;
                    existing.RotY = RotYViewModel.FromModel(notification.RotY);
                    existing.RotYYear = notification.RotYYear;
                    existing.Violation = ViolationViewModel.FromModel(notification.Violation);
                    existing.ViolationID = notification.ViolationID;
                }
            }
            NotificationItems.BatchCommit();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfNotifications(); });
            }
        }
    }
}
