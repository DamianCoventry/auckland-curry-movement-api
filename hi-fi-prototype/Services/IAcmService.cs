using acm_models;

namespace hi_fi_prototype.Services
{
    public interface IAcmService
    {
        bool IsSignedIn { get; }
        Task<bool> SignIn();
        void SignOut();
        string AccessToken { get; }

        Task<PageOfData<Attendee>> ListAttendeesAsync(int first, int count);
        Task<Attendee> GetAttendeeAsync(int ID);
        Task<Attendee> AddAttendeeAsync(Attendee x);
        Task<bool> UpdateAttendeeAsync(Attendee x);
        Task<bool> DeleteAttendeeAsync(int ID);

        Task<PageOfData<Club>> ListClubsAsync(int first, int count);
        Task<PageOfData<PastDinner>> ListClubPastDinnersAsync(int ID, int first, int count);
        Task<PageOfData<Member>> ListClubFoundingFathersAsync(int ID, int first, int count);
        Task<PageOfData<Member>> ListClubMembersAsync(int ID, int first, int count);
        Task<PageOfData<MemberStats>> ListClubMemberStatsAsync(int ID, int first, int count);
        Task<Club> GetClubAsync(int ID);
        Task<Club> AddClubAsync(Club x);
        Task<bool> UpdateClubAsync(Club x);
        Task<bool> DeleteClubAsync(int ID);

        Task<PageOfData<Dinner>> ListDinnersAsync(int first, int count);
        Task<List<AttendeeStats>?> ListDinnerAttendeeStatsAsync(int clubID, int dinnerID);
        Task<Dinner> GetDinnerAsync(int ID);
        Task<Dinner> AddDinnerAsync(Dinner x);
        Task<bool> UpdateDinnerAsync(Dinner x);
        Task<bool> DeleteDinnerAsync(int ID);

        Task<PageOfData<Exemption>> ListExemptionsAsync(int first, int count);
        Task<Exemption> GetExemptionAsync(int ID);
        Task<Exemption> AddExemptionAsync(Exemption x);
        Task<bool> UpdateExemptionAsync(Exemption x);
        Task<bool> DeleteExemptionAsync(int ID);

        Task<PageOfData<KotC>> ListKotCsAsync(int first, int count);
        Task<KotC> GetKotCAsync(int ID);
        Task<KotC> AddKotCAsync(KotC x);
        Task<bool> UpdateKotCAsync(KotC x);
        Task<bool> DeleteKotCAsync(int ID);

        Task<PageOfData<Level>> ListLevelsAsync(int first, int count);
        Task<Level> GetLevelAsync(int ID);
        Task<Level> AddLevelAsync(Level x);
        Task<bool> UpdateLevelAsync(Level x);
        Task<bool> DeleteLevelAsync(int ID);

        Task<PageOfData<Member>> ListMembersAsync(int first, int count);
        Task<Member> GetMemberAsync(int ID);
        Task<Member> AddMemberAsync(Member x);
        Task<bool> UpdateMemberAsync(Member x);
        Task<bool> DeleteMemberAsync(int ID);

        Task<PageOfData<Notification>> ListNotificationsAsync(int first, int count);
        Task<Notification> GetNotificationAsync(int ID);
        Task<Notification> AddNotificationAsync(Notification x);
        Task<bool> UpdateNotificationAsync(Notification x);
        Task<bool> DeleteNotificationAsync(int ID);

        Task<PageOfData<Reservation>> ListReservationsAsync(int first, int count);
        Task<Reservation> GetReservationAsync(int ID);
        Task<Reservation> AddReservationAsync(Reservation x);
        Task<bool> UpdateReservationAsync(Reservation x);
        Task<bool> DeleteReservationAsync(int ID);

        Task<PageOfData<Restaurant>> ListRestaurantsAsync(int first, int count);
        Task<Restaurant> GetRestaurantAsync(int ID);
        Task<RotYStats> GetRestaurantRotYStatsAsync(int ID);
        Task<Restaurant> AddRestaurantAsync(Restaurant x);
        Task<bool> UpdateRestaurantAsync(Restaurant x);
        Task<bool> DeleteRestaurantAsync(int ID);

        Task<PageOfData<RotY>> ListRotYsAsync(int first, int count);
        Task<RotY> GetRotYAsync(int ID);
        Task<RotY> AddRotYAsync(RotY x);
        Task<bool> UpdateRotYAsync(RotY x);
        Task<bool> DeleteRotYAsync(int ID);

        Task<PageOfData<Violation>> ListViolationsAsync(int first, int count);
        Task<Violation> GetViolationAsync(int ID);
        Task<Violation> AddViolationAsync(Violation x);
        Task<bool> UpdateViolationAsync(Violation x);
        Task<bool> DeleteViolationAsync(int ID);
    }
}
