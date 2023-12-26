using acm_models;
using Microsoft.Identity.Client;
using System.Net;
using System.Text;
using System.Text.Json;

namespace acm_mobile_app.Services
{
    internal class AcmService : IAcmService
    {
        private static readonly string ClientID = "a34405bc-327c-418c-96c1-a42d35ab0539";
        private static readonly string TenantID = "96bc6756-1530-4f30-80fe-016066953709";
        private static readonly string Scope = $"api://{ClientID}/user_impersonation";
        private static readonly string UriBase = "https://auckland-curry-movement.azurewebsites.net/api/";

        private readonly JsonSerializerOptions _serializerOptions;
        private readonly HttpClient _client;
        private readonly IPublicClientApplication _identityClient;

        private string _accessToken = string.Empty;

        public AcmService()
        {
            _client = new HttpClient();

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

#if ANDROID
            _identityClient = PublicClientApplicationBuilder
                .Create(ClientID)
                .WithAuthority(AzureCloudInstance.AzurePublic, TenantID)
                .WithRedirectUri($"msal{ClientID}://auth")
                .WithParentActivityOrWindow(() => Platform.CurrentActivity)
                .Build();
#elif IOS
            _identityClient = PublicClientApplicationBuilder
                .Create(ClientID)
                .WithAuthority(AzureCloudInstance.AzurePublic, TenantID)
                .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                .WithRedirectUri($"msal{ClientID}://auth")
                .Build();
#else
            _identityClient = PublicClientApplicationBuilder
                .Create(ClientID)
                .WithAuthority(AzureCloudInstance.AzurePublic, TenantID)
                .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
                .Build();
#endif
        }

        public bool IsSignedIn { get { return !string.IsNullOrEmpty(_accessToken); } }

        public async Task<bool> SignIn()
        {
            SignOut();
            await AcquireTokenIfRequired();
            return IsSignedIn;
        }

        public void SignOut()
        {
            _accessToken = string.Empty;
        }

        public string AccessToken { get { return _accessToken; } }

        private struct ListItemsResult<T>
        {
            public List<T>? _items;
            public HttpStatusCode _statusCode;
        }

        private struct MultipleItemsResult<T>
        {
            public PageOfData<T>? _pageOfData;
            public HttpStatusCode _statusCode;
        }

        private struct SingleItemResult<T>
        {
            public T? _item;
            public HttpStatusCode _statusCode;
        }

        private enum TokenResult { Acquired, UiRequired }

        private async Task<TokenResult> AcquireTokenSilently()
        {
            TokenResult tokenResult = TokenResult.Acquired;
            try
            {
                var accounts = await _identityClient.GetAccountsAsync();

                var statusCode = await _identityClient
                    .AcquireTokenSilent(new string[] { Scope }, accounts.FirstOrDefault())
                    .ExecuteAsync();

                if (statusCode == null || statusCode.AccessToken == null)
                    throw new Exception("Unable to silently acquire an access token");

                _accessToken = statusCode.AccessToken;
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
            }
            catch (MsalUiRequiredException)
            {
                tokenResult = TokenResult.UiRequired;
            }

            return tokenResult;
        }

        private async Task<TokenResult> AcquireTokenInteractively()
        {
            var statusCode = await _identityClient
                .AcquireTokenInteractive(new string[] { Scope })
                .ExecuteAsync();

            if (statusCode == null || statusCode.AccessToken == null)
                throw new Exception("Unable to interactively acquire an access token");

            _accessToken = statusCode.AccessToken;
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            return TokenResult.Acquired;
        }

        private async Task AcquireTokenIfRequired()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                if (await AcquireTokenSilently() == TokenResult.UiRequired)
                    await AcquireTokenInteractively();
            }
        }

        public static bool IsSuccessfulStatusCode(HttpStatusCode statusCode)
        {
            return ((int)statusCode >= 200) && ((int)statusCode <= 299);
        }

        private async Task<PageOfData<T>> ListItems<T>(string path, int first, int count)
        {
            await AcquireTokenIfRequired();

            var itemList = await ListItemsInternal<T>(path, first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            itemList = await ListItemsInternal<T>(path, first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");
        }

        private async Task<MultipleItemsResult<T>> ListItemsInternal<T>(string path, int first, int count)
        {
            Uri uri = new($"{UriBase}{path}?first={first}&count={count}");

            HttpResponseMessage response = await _client.GetAsync(uri)
                ?? throw new Exception("Unable to request a list of items");

            MultipleItemsResult<T> statusCode = new() { _statusCode = response.StatusCode };

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync()
                    ?? throw new Exception("Unable to read returned data");

                statusCode._pageOfData = JsonSerializer.Deserialize<PageOfData<T>>(content, _serializerOptions)
                    ?? throw new Exception("Unable to parse returned data as JSON");
            }

            return statusCode;
        }

        private async Task<ListItemsResult<T>> ListInternal<T>(string path)
        {
            Uri uri = new($"{UriBase}{path}");

            HttpResponseMessage response = await _client.GetAsync(uri)
                ?? throw new Exception("Unable to request a list of items");

            ListItemsResult<T> statusCode = new() { _statusCode = response.StatusCode };

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync()
                    ?? throw new Exception("Unable to read returned data");

                statusCode._items = JsonSerializer.Deserialize<List<T>>(content, _serializerOptions)
                    ?? throw new Exception("Unable to parse returned data as JSON");
            }

            return statusCode;
        }

        private async Task<T> GetItem<T>(string path, int ID)
        {
            await AcquireTokenIfRequired();

            var item = await GetItemInternal<T>(path, ID);
            if (IsSuccessfulStatusCode(item._statusCode) && item._item != null)
                return item._item;

            if (item._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to retrieve an item ({item._statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            item = await GetItemInternal<T>(path, ID);
            if (IsSuccessfulStatusCode(item._statusCode) && item._item != null)
                return item._item;

            if (item._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to retrieve an item ({item._statusCode})");
        }

        private async Task<SingleItemResult<T>> GetItemInternal<T>(string path, int ID)
        {
            Uri uri = new($"{UriBase}{path}/{ID}");

            HttpResponseMessage response = await _client.GetAsync(uri)
                ?? throw new Exception("Unable to request an item");

            SingleItemResult<T> statusCode = new() { _statusCode = response.StatusCode };

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync()
                    ?? throw new Exception("Unable to read returned data");

                statusCode._item = JsonSerializer.Deserialize<T>(content, _serializerOptions)
                    ?? throw new Exception("Unable to parse returned data as JSON");
            }

            return statusCode;
        }

        private async Task<SingleItemResult<T>> GetInternal<T>(string path)
        {
            Uri uri = new($"{UriBase}{path}");

            HttpResponseMessage response = await _client.GetAsync(uri)
                ?? throw new Exception("Unable to request an item");

            SingleItemResult<T> statusCode = new() { _statusCode = response.StatusCode };

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync()
                    ?? throw new Exception("Unable to read returned data");

                statusCode._item = JsonSerializer.Deserialize<T>(content, _serializerOptions)
                    ?? throw new Exception("Unable to parse returned data as JSON");
            }

            return statusCode;
        }

        private async Task<T> AddItem<T>(string path, T item)
        {
            await AcquireTokenIfRequired();

            var itemResult = await AddItemInternal<T>(path, item);
            if (IsSuccessfulStatusCode(itemResult._statusCode) && itemResult._item != null)
                return itemResult._item;

            if (itemResult._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to add an item ({itemResult._statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            itemResult = await AddItemInternal(path, item);
            if (IsSuccessfulStatusCode(itemResult._statusCode) && itemResult._item != null)
                return itemResult._item;

            if (itemResult._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to add an item ({itemResult._statusCode})");
        }

        private async Task<SingleItemResult<T>> AddItemInternal<T>(string path, T item)
        {
            string json = JsonSerializer.Serialize<T>(item, _serializerOptions);
            StringContent requestContent = new(json, Encoding.UTF8, "application/json");

            Uri uri = new($"{UriBase}{path}");

            HttpResponseMessage response = await _client.PostAsync(uri, requestContent)
                ?? throw new Exception("Unable to add an item");

            SingleItemResult<T> statusCode = new() { _statusCode = response.StatusCode };

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync()
                    ?? throw new Exception("Unable to read returned data");

                statusCode._item = JsonSerializer.Deserialize<T>(responseContent, _serializerOptions)
                    ?? throw new Exception("Unable to parse returned data as JSON");
            }

            return statusCode;
        }

        private async Task<bool> UpdateItem<T>(string path, int ID, T item)
        {
            await AcquireTokenIfRequired();

            var statusCode = await UpdateItemInternal(path, ID, item);
            if (IsSuccessfulStatusCode(statusCode))
                return true;

            if (statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to update an item ({statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            statusCode = await UpdateItemInternal(path, ID, item);
            if (IsSuccessfulStatusCode(statusCode))
                return true;

            if (statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to update an item ({statusCode})");
        }

        private async Task<HttpStatusCode> UpdateItemInternal<T>(string path, int ID, T item)
        {
            string json = JsonSerializer.Serialize<T>(item, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            Uri uri = new($"{UriBase}{path}/{ID}");

            HttpResponseMessage response = await _client.PutAsync(uri, content)
                ?? throw new Exception("Unable to update an item");

            return response.StatusCode;
        }

        private async Task<bool> DeleteItem(string path, int ID)
        {
            await AcquireTokenIfRequired();

            var statusCode = await DeleteItemInternal(path, ID);
            if (IsSuccessfulStatusCode(statusCode))
                return true;

            if (statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to delete an item ({statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            statusCode = await DeleteItemInternal(path, ID);
            if (IsSuccessfulStatusCode(statusCode))
                return true;

            if (statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to delete an item ({statusCode})");
        }

        private async Task<HttpStatusCode> DeleteItemInternal(string path, int ID)
        {
            Uri uri = new($"{UriBase}{path}/{ID}");

            HttpResponseMessage response = await _client.DeleteAsync(uri)
                ?? throw new Exception("Unable to delete an item");

            return response.StatusCode;
        }

        public Task<Attendee> AddAttendeeAsync(Attendee x)
        {
            return AddItem("Attendee", x);
        }

        public Task<Club> AddClubAsync(Club x)
        {
            return AddItem("Club", x);
        }

        public Task<Dinner> AddDinnerAsync(Dinner x)
        {
            return AddItem("Dinner", x);
        }

        public Task<Exemption> AddExemptionAsync(Exemption x)
        {
            return AddItem("Exemption", x);
        }

        public Task<KotC> AddKotCAsync(KotC x)
        {
            return AddItem("KotC", x);
        }

        public Task<Level> AddLevelAsync(Level x)
        {
            return AddItem("Level", x);
        }

        public Task<Member> AddMemberAsync(Member x)
        {
            return AddItem("Member", x);
        }

        public Task<Notification> AddNotificationAsync(Notification x)
        {
            return AddItem("Notification", x);
        }

        public Task<Reservation> AddReservationAsync(Reservation x)
        {
            return AddItem("Reservation", x);
        }

        public Task<Restaurant> AddRestaurantAsync(Restaurant x)
        {
            return AddItem("Restaurant", x);
        }

        public Task<RotY> AddRotYAsync(RotY x)
        {
            return AddItem("RotY", x);
        }

        public Task<Violation> AddViolationAsync(Violation x)
        {
            return AddItem("Violation", x);
        }

        public Task<bool> DeleteAttendeeAsync(int ID)
        {
            return DeleteItem("Attendee", ID);
        }

        public Task<bool> DeleteClubAsync(int ID)
        {
            return DeleteItem("Club", ID);
        }

        public Task<bool> DeleteDinnerAsync(int ID)
        {
            return DeleteItem("Dinner", ID);
        }

        public Task<bool> DeleteExemptionAsync(int ID)
        {
            return DeleteItem("Exemption", ID);
        }

        public Task<bool> DeleteKotCAsync(int ID)
        {
            return DeleteItem("KotC", ID);
        }

        public Task<bool> DeleteLevelAsync(int ID)
        {
            return DeleteItem("Level", ID);
        }

        public Task<bool> DeleteMemberAsync(int ID)
        {
            return DeleteItem("Member", ID);
        }

        public Task<bool> DeleteNotificationAsync(int ID)
        {
            return DeleteItem("Notification", ID);
        }

        public Task<bool> DeleteReservationAsync(int ID)
        {
            return DeleteItem("Reservation", ID);
        }

        public Task<bool> DeleteRestaurantAsync(int ID)
        {
            return DeleteItem("Restaurant", ID);
        }

        public Task<bool> DeleteRotYAsync(int ID)
        {
            return DeleteItem("RotY", ID);
        }

        public Task<bool> DeleteViolationAsync(int ID)
        {
            return DeleteItem("Violation", ID);
        }

        public Task<Attendee> GetAttendeeAsync(int ID)
        {
            return GetItem<Attendee>("Attendee", ID);
        }

        public Task<Club> GetClubAsync(int ID)
        {
            return GetItem<Club>("Club", ID);
        }

        public Task<Dinner> GetDinnerAsync(int ID)
        {
            return GetItem<Dinner>("Dinner", ID);
        }

        public Task<Exemption> GetExemptionAsync(int ID)
        {
            return GetItem<Exemption>("Exemption", ID);
        }

        public Task<KotC> GetKotCAsync(int ID)
        {
            return GetItem<KotC>("KotC", ID);
        }

        public Task<Level> GetLevelAsync(int ID)
        {
            return GetItem<Level>("Level", ID);
        }

        public Task<Member> GetMemberAsync(int ID)
        {
            return GetItem<Member>("Member", ID);
        }

        public Task<Notification> GetNotificationAsync(int ID)
        {
            return GetItem<Notification>("Notification", ID);
        }

        public Task<Reservation> GetReservationAsync(int ID)
        {
            return GetItem<Reservation>("Reservation", ID);
        }

        public Task<Restaurant> GetRestaurantAsync(int ID)
        {
            return GetItem<Restaurant>("Restaurant", ID);
        }

        public async Task<RotYStats> GetRestaurantRotYStatsAsync(int ID)
        {
            await AcquireTokenIfRequired();

            var item = await GetInternal<RotYStats>($"Restaurant/{ID}/RotYStats");
            if (IsSuccessfulStatusCode(item._statusCode) && item._item != null)
                return item._item;

            if (item._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to retrieve an item ({item._statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            item = await GetInternal<RotYStats>($"Restaurant/{ID}/RotYStats");
            if (IsSuccessfulStatusCode(item._statusCode) && item._item != null)
                return item._item;

            if (item._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to retrieve an item ({item._statusCode})");
        }

        public Task<RotY> GetRotYAsync(int ID)
        {
            return GetItem<RotY>("RotY", ID);
        }

        public Task<Violation> GetViolationAsync(int ID)
        {
            return GetItem<Violation>("Violation", ID);
        }

        public Task<PageOfData<Attendee>> ListAttendeesAsync(int first, int count)
        {
            return ListItems<Attendee>("Attendee", first, count);
        }

        public Task<PageOfData<Club>> ListClubsAsync(int first, int count)
        {
            return ListItems<Club>("Club", first, count);
        }

        public async Task<PageOfData<PastDinner>> ListClubPastDinnersAsync(int ID, int first, int count)
        {
            await AcquireTokenIfRequired();

            var itemList = await ListItemsInternal<PastDinner>($"Club/{ID}/PastDinners", first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            itemList = await ListItemsInternal<PastDinner>($"Club/{ID}/PastDinners", first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");
        }

        public async Task<PageOfData<Member>> ListClubFoundingFathersAsync(int ID, int first, int count)
        {
            await AcquireTokenIfRequired();

            var itemList = await ListItemsInternal<Member>($"Club/{ID}/FoundingFathers", first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            itemList = await ListItemsInternal<Member>($"Club/{ID}/FoundingFathers", first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");
        }

        public async Task<PageOfData<Member>> ListClubMembersAsync(int ID, int first, int count)
        {
            await AcquireTokenIfRequired();

            var itemList = await ListItemsInternal<Member>($"Club/{ID}/Members", first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            itemList = await ListItemsInternal<Member>($"Club/{ID}/Members", first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");
        }

        public async Task<PageOfData<MemberStats>> ListClubMemberStatsAsync(int ID, int first, int count)
        {
            await AcquireTokenIfRequired();

            var itemList = await ListItemsInternal<MemberStats>($"Club/{ID}/MemberStats", first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            itemList = await ListItemsInternal<MemberStats>($"Club/{ID}/Members", first, count);
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._pageOfData != null)
                return itemList._pageOfData;

            if (itemList._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");
        }

        public Task<PageOfData<Dinner>> ListDinnersAsync(int first, int count)
        {
            return ListItems<Dinner>("Dinner", first, count);
        }

        public async Task<List<AttendeeStats>?> ListDinnerAttendeeStatsAsync(int clubID, int dinnerID)
        {
            await AcquireTokenIfRequired();

            var itemList = await ListInternal<AttendeeStats>($"Dinner/{dinnerID}/Attendees?clubID={clubID}");
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._items != null)
                return itemList._items;

            if (itemList._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            itemList = await ListInternal<AttendeeStats>($"Dinner/{dinnerID}/Attendees?clubID={clubID}");
            if (IsSuccessfulStatusCode(itemList._statusCode) && itemList._items != null)
                return itemList._items;

            if (itemList._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception($"Unable to retrieve a list of items ({itemList._statusCode})");
        }

        public Task<PageOfData<Exemption>> ListExemptionsAsync(int first, int count)
        {
            return ListItems<Exemption>("Exemption", first, count);
        }

        public Task<PageOfData<KotC>> ListKotCsAsync(int first, int count)
        {
            return ListItems<KotC>("KotC", first, count);
        }

        public Task<PageOfData<Level>> ListLevelsAsync(int first, int count)
        {
            return ListItems<Level>("Level", first, count);
        }

        public Task<PageOfData<Member>> ListMembersAsync(int first, int count)
        {
            return ListItems<Member>("Member", first, count);
        }

        public Task<PageOfData<Notification>> ListNotificationsAsync(int first, int count)
        {
            return ListItems<Notification>("Notification", first, count);
        }

        public Task<PageOfData<Reservation>> ListReservationsAsync(int first, int count)
        {
            return ListItems<Reservation>("Reservation", first, count);
        }

        public Task<PageOfData<Restaurant>> ListRestaurantsAsync(int first, int count)
        {
            return ListItems<Restaurant>("Restaurant", first, count);
        }

        public Task<PageOfData<RotY>> ListRotYsAsync(int first, int count)
        {
            return ListItems<RotY>("RotY", first, count);
        }

        public Task<PageOfData<Violation>> ListViolationsAsync(int first, int count)
        {
            return ListItems<Violation>("Violation", first, count);
        }

        public Task<bool> UpdateAttendeeAsync(Attendee x)
        {
            return UpdateItem("Attendee", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateClubAsync(Club x)
        {
            return UpdateItem("Club", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateDinnerAsync(Dinner x)
        {
            return UpdateItem("Dinner", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateExemptionAsync(Exemption x)
        {
            return UpdateItem("Exemption", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateKotCAsync(KotC x)
        {
            return UpdateItem("KotC", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateLevelAsync(Level x)
        {
            return UpdateItem("Level", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateMemberAsync(Member x)
        {
            return UpdateItem("Member", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateNotificationAsync(Notification x)
        {
            return UpdateItem("Notification", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateReservationAsync(Reservation x)
        {
            return UpdateItem("Reservation", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateRestaurantAsync(Restaurant x)
        {
            return UpdateItem("Restaurant", x.ID == null ? 0 : (int)x.ID, x);
        }

        public Task<bool> UpdateRotYAsync(RotY x)
        {
            return UpdateItem("RotY", x.Year == null ? 0 : (int)x.Year, x);
        }

        public Task<bool> UpdateViolationAsync(Violation x)
        {
            return UpdateItem("Violation", x.ID == null ? 0 : (int)x.ID, x);
        }
    }
}
