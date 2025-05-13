using System.Net.Http.Json;
using System.Net.Http.Headers;
using AvstickareBlazor.Models;
using Blazored.LocalStorage;
using System.Text.Json;

namespace AvstickareBlazor
{
    public class FavoriteService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public FavoriteService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        //lägg till favorit
        public async Task AddToFavoritesAsync(string placeId)
        {
            if (string.IsNullOrWhiteSpace(placeId))
                throw new ArgumentException("Plats-ID saknas.");

            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Du är inte inloggad.");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/FavoritePlace")
            {
                Content = JsonContent.Create(new FavoritePlaceRequest { MapServicePlaceId = placeId })
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Misslyckades med att spara favoritplats");
            }
        }

        //kolla om favoriten redan är sparad
        public async Task<bool> IsFavoriteAsync(string mapServicePlaceId)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new UnauthorizedAccessException("Du är inte inloggad.");
            }
                
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/FavoritePlace/exists/{mapServicePlaceId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Kunde inte kontrollera favoritstatus.");
            }

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("isFavorite").GetBoolean();
        }


        //ta bort favorit
        public async Task RemoveFromFavoritesAsync(string mapServicePlaceId)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Du är inte inloggad.");

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/FavoritePlace/remove/{mapServicePlaceId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Misslyckades ta bort favoritplats.");
            }
        }
    }
}