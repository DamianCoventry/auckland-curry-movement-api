namespace acm_mobile_app.Services
{
    internal interface IAcmService
    {
        bool IsSignedIn { get; }
        Task<bool> SignIn();
        void SignOut();

        Task<List<Models.Attendee>> ListAttendeesAsync(int first, int count);
        Task<Models.Attendee> GetAttendeeAsync(int ID);
        Task<Models.Attendee> AddAttendeeAsync(Models.Attendee x);
        Task UpdateAttendeeAsync(Models.Attendee x);
        Task DeleteAttendeeAsync(int ID);

        Task<List<Models.Club>> ListClubsAsync(int first, int count);
        Task<Models.Club> GetClubAsync(int ID);
        Task<Models.Club> AddClubAsync(Models.Club x);
        Task UpdateClubAsync(Models.Club x);
        Task DeleteClubAsync(int ID);

        Task<List<Models.Dinner>> ListDinnersAsync(int first, int count);
        Task<Models.Dinner> GetDinnerAsync(int ID);
        Task<Models.Dinner> AddDinnerAsync(Models.Dinner x);
        Task UpdateDinnerAsync(Models.Dinner x);
        Task DeleteDinnerAsync(int ID);

        Task<List<Models.Exemption>> ListExemptionsAsync(int first, int count);
        Task<Models.Exemption> GetExemptionAsync(int ID);
        Task<Models.Exemption> AddExemptionAsync(Models.Exemption x);
        Task UpdateExemptionAsync(Models.Exemption x);
        Task DeleteExemptionAsync(int ID);

        Task<List<Models.KotC>> ListKotCsAsync(int first, int count);
        Task<Models.KotC> GetKotCAsync(int ID);
        Task<Models.KotC> AddKotCAsync(Models.KotC x);
        Task UpdateKotCAsync(Models.KotC x);
        Task DeleteKotCAsync(int ID);

        Task<List<Models.Level>> ListLevelsAsync(int first, int count);
        Task<Models.Level> GetLevelAsync(int ID);
        Task<Models.Level> AddLevelAsync(Models.Level x);
        Task UpdateLevelAsync(Models.Level x);
        Task DeleteLevelAsync(int ID);

        Task<List<Models.Member>> ListMembersAsync(int first, int count);
        Task<Models.Member> GetMemberAsync(int ID);
        Task<Models.Member> AddMemberAsync(Models.Member x);
        Task UpdateMemberAsync(Models.Member x);
        Task DeleteMemberAsync(int ID);

        Task<List<Models.Notification>> ListNotificationsAsync(int first, int count);
        Task<Models.Notification> GetNotificationAsync(int ID);
        Task<Models.Notification> AddNotificationAsync(Models.Notification x);
        Task UpdateNotificationAsync(Models.Notification x);
        Task DeleteNotificationAsync(int ID);

        Task<List<Models.Reservation>> ListReservationsAsync(int first, int count);
        Task<Models.Reservation> GetReservationAsync(int ID);
        Task<Models.Reservation> AddReservationAsync(Models.Reservation x);
        Task UpdateReservationAsync(Models.Reservation x);
        Task DeleteReservationAsync(int ID);

        Task<List<Models.Restaurant>> ListRestaurantsAsync(int first, int count);
        Task<Models.Restaurant> GetRestaurantAsync(int ID);
        Task<Models.Restaurant> AddRestaurantAsync(Models.Restaurant x);
        Task UpdateRestaurantAsync(Models.Restaurant x);
        Task DeleteRestaurantAsync(int ID);

        Task<List<Models.RotY>> ListRotYsAsync(int first, int count);
        Task<Models.RotY> GetRotYAsync(int ID);
        Task<Models.RotY> AddRotYAsync(Models.RotY x);
        Task UpdateRotYAsync(Models.RotY x);
        Task DeleteRotYAsync(int ID);

        Task<List<Models.Violation>> ListViolationsAsync(int first, int count);
        Task<Models.Violation> GetViolationAsync(int ID);
        Task<Models.Violation> AddViolationAsync(Models.Violation x);
        Task UpdateViolationAsync(Models.Violation x);
        Task DeleteViolationAsync(int ID);
    }
}
