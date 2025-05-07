using System.Net.Http.Json;
using AvstickareBlazor.Models;

namespace AvstickareBlazor
{
    public class TripService(HttpClient http)
    {
        private readonly HttpClient _http = http;

        public TripPlanResult? LatestTrip { get; set; }

        //anropa backend för att 
        public async Task<TripPlanResult> PlanTripAsync(string? fromLocation, string? toLocation)
        {
            var request = new PlanTripRequest
            {
                FromLocation = fromLocation,
                ToLocation = toLocation
            };

            var response = await _http.PostAsJsonAsync("http://localhost:5013/api/Trip/plan", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Det uppstod ett fel.");
            }

            var result = await response.Content.ReadFromJsonAsync<TripPlanResult>();

            if (result == null)
            {
                throw new Exception("Det uppstod ett fel.");
            }

            LatestTrip = result;
            return result;
        }
    }
}

