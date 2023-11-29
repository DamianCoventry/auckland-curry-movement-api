using acm_mobile_app.Models;
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

        private struct MultipleItemsResult<T>
        {
            public List<T>? _items;
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

        private async Task<List<T>> ListItems<T>(string path, int first, int count)
        {
            await AcquireTokenIfRequired();

            var statusCode = await ListItemsInternal<T>(path, first, count);
            if (IsSuccessfulStatusCode(statusCode._statusCode) && statusCode._items != null)
                return statusCode._items;

            if (statusCode._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception("Unable to retrieve a list of items");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            statusCode = await ListItemsInternal<T>(path, first, count);
            if (IsSuccessfulStatusCode(statusCode._statusCode) && statusCode._items != null)
                return statusCode._items;

            if (statusCode._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception("Unable to retrieve a list of items");
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

                statusCode._items = JsonSerializer.Deserialize<List<T>>(content, _serializerOptions)
                    ?? throw new Exception("Unable to parse returned data as JSON");
            }

            return statusCode;
        }

        private async Task<T> GetItem<T>(string path, int ID)
        {
            await AcquireTokenIfRequired();

            var statusCode = await GetItemInternal<T>(path, ID);
            if (IsSuccessfulStatusCode(statusCode._statusCode) && statusCode._item != null)
                return statusCode._item;

            if (statusCode._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception("Unable to retrieve an item");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            statusCode = await GetItemInternal<T>(path, ID);
            if (IsSuccessfulStatusCode(statusCode._statusCode) && statusCode._item != null)
                return statusCode._item;

            if (statusCode._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception("Unable to retrieve an item");
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

        private async Task<T> AddItem<T>(string path, T item)
        {
            await AcquireTokenIfRequired();

            var statusCode = await AddItemInternal<T>(path, item);
            if (IsSuccessfulStatusCode(statusCode._statusCode) && statusCode._item != null)
                return statusCode._item;

            if (statusCode._statusCode != HttpStatusCode.Unauthorized)
                throw new Exception("Unable to add an item");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            statusCode = await AddItemInternal(path, item);
            if (IsSuccessfulStatusCode(statusCode._statusCode) && statusCode._item != null)
                return statusCode._item;

            if (statusCode._statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception("Unable to add an item");
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

        private async Task<bool> UpdateItem<T>(string path, T item)
        {
            await AcquireTokenIfRequired();

            var statusCode = await UpdateItemInternal<T>(path, item);
            if (IsSuccessfulStatusCode(statusCode))
                return true;

            if (statusCode != HttpStatusCode.Unauthorized)
                throw new Exception("Unable to update an item");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            statusCode = await UpdateItemInternal(path, item);
            if (IsSuccessfulStatusCode(statusCode))
                return true;

            if (statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception("Unable to update an item");
        }

        private async Task<HttpStatusCode> UpdateItemInternal<T>(string path, T item)
        {
            string json = JsonSerializer.Serialize<T>(item, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            Uri uri = new($"{UriBase}{path}");

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
                throw new Exception("Unable to delete an item");

            // Get a new one.
            _accessToken = string.Empty;
            await AcquireTokenInteractively();

            statusCode = await DeleteItemInternal(path, ID);
            if (IsSuccessfulStatusCode(statusCode))
                return true;

            if (statusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new Exception("Unable to delete an item");
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

        public Task DeleteAttendeeAsync(int ID)
        {
            return DeleteItem("Attendee", ID);
        }

        public Task DeleteClubAsync(int ID)
        {
            return DeleteItem("Club", ID);
        }

        public Task DeleteDinnerAsync(int ID)
        {
            return DeleteItem("Dinner", ID);
        }

        public Task DeleteExemptionAsync(int ID)
        {
            return DeleteItem("Exemption", ID);
        }

        public Task DeleteKotCAsync(int ID)
        {
            return DeleteItem("KotC", ID);
        }

        public Task DeleteLevelAsync(int ID)
        {
            return DeleteItem("Level", ID);
        }

        public Task DeleteMemberAsync(int ID)
        {
            return DeleteItem("Member", ID);
        }

        public Task DeleteNotificationAsync(int ID)
        {
            return DeleteItem("Notification", ID);
        }

        public Task DeleteReservationAsync(int ID)
        {
            return DeleteItem("Reservation", ID);
        }

        public Task DeleteRestaurantAsync(int ID)
        {
            return DeleteItem("Restaurant", ID);
        }

        public Task DeleteRotYAsync(int ID)
        {
            return DeleteItem("RotY", ID);
        }

        public Task DeleteViolationAsync(int ID)
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

        public Task<RotY> GetRotYAsync(int ID)
        {
            return GetItem<RotY>("RotY", ID);
        }

        public Task<Violation> GetViolationAsync(int ID)
        {
            return GetItem<Violation>("Violation", ID);
        }

        public Task<List<Attendee>> ListAttendeesAsync(int first, int count)
        {
            return ListItems<Attendee>("Attendee", first, count);
        }

        public Task<List<Club>> ListClubsAsync(int first, int count)
        {
            return ListItems<Club>("Club", first, count);
        }

        public Task<List<Dinner>> ListDinnersAsync(int first, int count)
        {
            return ListItems<Dinner>("Dinner", first, count);
        }

        public Task<List<Exemption>> ListExemptionsAsync(int first, int count)
        {
            return ListItems<Exemption>("Exemption", first, count);
        }

        public Task<List<KotC>> ListKotCsAsync(int first, int count)
        {
            return ListItems<KotC>("KotC", first, count);
        }

        public Task<List<Level>> ListLevelsAsync(int first, int count)
        {
            return ListItems<Level>("Level", first, count);
        }

        public Task<List<Member>> ListMembersAsync(int first, int count)
        {
            return ListItems<Member>("Member", first, count);
        }

        public Task<List<Notification>> ListNotificationsAsync(int first, int count)
        {
            return ListItems<Notification>("Notification", first, count);
        }

        public Task<List<Reservation>> ListReservationsAsync(int first, int count)
        {
            return ListItems<Reservation>("Reservation", first, count);
        }

        public Task<List<Restaurant>> ListRestaurantsAsync(int first, int count)
        {
            return ListItems<Restaurant>("Restaurant", first, count);
        }

        public Task<List<RotY>> ListRotYsAsync(int first, int count)
        {
            return ListItems<RotY>("RotY", first, count);
        }

        public Task<List<Violation>> ListViolationsAsync(int first, int count)
        {
            return ListItems<Violation>("Violation", first, count);
        }

        public Task UpdateAttendeeAsync(Attendee x)
        {
            return UpdateItem("Attendee", x);
        }

        public Task UpdateClubAsync(Club x)
        {
            return UpdateItem("Club", x);
        }

        public Task UpdateDinnerAsync(Dinner x)
        {
            return UpdateItem("Dinner", x);
        }

        public Task UpdateExemptionAsync(Exemption x)
        {
            return UpdateItem("Exemption", x);
        }

        public Task UpdateKotCAsync(KotC x)
        {
            return UpdateItem("KotC", x);
        }

        public Task UpdateLevelAsync(Level x)
        {
            return UpdateItem("Level", x);
        }

        public Task UpdateMemberAsync(Member x)
        {
            return UpdateItem("Member", x);
        }

        public Task UpdateNotificationAsync(Notification x)
        {
            return UpdateItem("Notification", x);
        }

        public Task UpdateReservationAsync(Reservation x)
        {
            return UpdateItem("Reservation", x);
        }

        public Task UpdateRestaurantAsync(Restaurant x)
        {
            return UpdateItem("Restaurant", x);
        }

        public Task UpdateRotYAsync(RotY x)
        {
            return UpdateItem("RotY", x);
        }

        public Task UpdateViolationAsync(Violation x)
        {
            return UpdateItem("Violation", x);
        }
    }
}
