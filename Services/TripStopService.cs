using System.Net.Http.Json;
using AvstickareBlazor.Models;

namespace AvstickareBlazor
{
    public class TripStopService(HttpClient http)
    {
        private readonly HttpClient _http = http;

        //håller temporära stopp som lista till resan sparas.
        public List<string> TemporaryStops {get; set;} = [];

        //lägg till tillfälliga stopp som inte redan finns innan listan sparas
        public void AddTemporaryStop(string mapServicePlaceId)
        {
            if(!TemporaryStops.Contains(mapServicePlaceId))
            {
                TemporaryStops.Add(mapServicePlaceId);
            }
        }

        //vilka stopp som finns i den temporära listan
        public bool HasTemporaryStop(string mapServicePlaceId)
        {
            return TemporaryStops.Contains(mapServicePlaceId);
        }

        //ta bort temporära stopp
        public void RemoveTemporaryStop(string mapServicePlaceId)
        {
            TemporaryStops.Remove(mapServicePlaceId);
        }

        public async Task<bool> AddStopAsync(int tripId, string mapServicePlaceId)
        {
            var tripStop = new TripStopFront
            {
                TripId = tripId,
                MapServicePlaceId = mapServicePlaceId
            };

            var response = await _http.PostAsJsonAsync("api/TripStop", tripStop);
            return response.IsSuccessStatusCode;
        }
    }
}