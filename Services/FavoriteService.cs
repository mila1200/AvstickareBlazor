using System.Net.Http.Json;
using AvstickareBlazor.Models;

namespace AvstickareBlazor
{
    public class FavoriteService(HttpClient http)
    {
        private readonly HttpClient _http = http;

      public async Task AddToFavoritesAsync(string placeId)
{
    if (string.IsNullOrWhiteSpace(placeId))
        throw new ArgumentException("Plats-ID saknas.");

    var body = new FavoritePlaceRequest
    {
        MapServicePlaceId = placeId
    };

    var response = await _http.PostAsJsonAsync("/api/FavoritePlace", body);

    if (!response.IsSuccessStatusCode)
    {
        var msg = await response.Content.ReadAsStringAsync();
        throw new Exception($"Misslyckades spara favorit: {msg}");
    }
}
    }
}