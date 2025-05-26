using System.Net.Http.Json;
using AvstickareBlazor.Models;

namespace AvstickareBlazor
{
    public class PlaceService(HttpClient http)
    {
        private readonly HttpClient _http = http;

        //för att hämta detaljerad info om platser via backend
        public async Task<PlaceDetailsFront?> GetPlaceDetailsAsync(string mapServicePlaceId)
        {
            
            var response = await _http.GetAsync($"api/Place/{mapServicePlaceId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Kunde inte hämta platsinformation.");
            }

            var place = await response.Content.ReadFromJsonAsync<PlaceDetailsFront>();
            
            if (place == null)
            {
                throw new Exception("Det saknas information om platsen.");
            }

            return place;
        }
    }
}