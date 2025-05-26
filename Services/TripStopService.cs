using System.Net.Http.Json;
using AvstickareBlazor.Models;
using Blazored.LocalStorage;
using AvstickareBlazor.Helpers;

namespace AvstickareBlazor
{
    public class TripStopService(HttpClient http, ILocalStorageService localStorage)
    {
        private readonly HttpClient _http = http;
        private readonly ILocalStorageService _localStorage = localStorage;

        //nyckeln som anvönds i local storage 
        private const string StorageKey = "tripDraft";

        //användarens pågående reseutkast (start, slut, valda stopp)
        public TripDraft Draft { get; set; } = new();
        
        //ladda från localStorage
        public async Task LoadAsync()
        {
            Draft = await _localStorage.GetItemAsync<TripDraft>(StorageKey) ?? new TripDraft();
        }

        //spara till localStorage
        public async Task SaveAsync()
        {
            await _localStorage.SetItemAsync(StorageKey, Draft);
        }

        //ladda detaljer för stopp
        public async Task<List<PlaceDetailsFront>> GetTemporaryStops()
        {
            await LoadAsync();

            var stops = new List<PlaceDetailsFront>();

            if (string.IsNullOrWhiteSpace(Draft.FromPlaceId))
            {
                return stops;
            }

            // hämta info om fromPlace
            var from = await _http.GetFromJsonAsync<PlaceDetailsFront>($"api/Place/{Draft.FromPlaceId}");

            if (from == null || from.Latitude == null || from.Longitude == null)
            {
                return stops;
            }

            var calculatedStops = new List<(PlaceDetailsFront Stop, double Distance)>();

            foreach (var id in Draft.StopPlaceIds)
            {
                try
                {
                    var stop = await _http.GetFromJsonAsync<PlaceDetailsFront>($"api/Place/{id}");
                    //beräkna avstånd
                    if (stop?.Latitude != null && stop.Longitude != null)
                    {
                        var distance = DistanceCalculator.Haversine(
                            from.Latitude.Value,
                            from.Longitude.Value,
                            stop.Latitude.Value,
                            stop.Longitude.Value
                        );

                        calculatedStops.Add((stop, distance));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Kunde inte hämta plats {id}: {ex.Message}");
                }
            }

            //sortera efter avstånd
            var sorted = calculatedStops.OrderBy(e => e.Distance).ToList();

            //numrera i rätt ordning
            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].Stop.Order = i + 1;
                stops.Add(sorted[i].Stop);
            }

            return stops;
        }

        //lägg till stopp
        public async Task AddTemporaryStop(string mapServicePlaceId)
        {
            if (!Draft.StopPlaceIds.Contains(mapServicePlaceId))
            {
                Draft.StopPlaceIds.Add(mapServicePlaceId);
                await SaveAsync();
            }
        }

        //ta bort stopp
        public async Task RemoveTemporaryStop(string mapServicePlaceId)
        {
            if (Draft.StopPlaceIds.Remove(mapServicePlaceId))
            {
                await SaveAsync();
            }
        }

        //kontrollera om stopp finns
        public bool HasTemporaryStop(string mapServicePlaceId)
        {
            return Draft.StopPlaceIds.Contains(mapServicePlaceId);
        }

        //sätt from/till
        public async Task SetFromPlaceAsync(string placeId)
        {
            Draft.FromPlaceId = placeId;
            await SaveAsync();
        }

        public async Task SetToPlaceAsync(string placeId)
        {
            Draft.ToPlaceId = placeId;
            await SaveAsync();
        }

        //hämta From/To
        public string? GetFromPlaceId() => Draft.FromPlaceId;
        public string? GetToPlaceId() => Draft.ToPlaceId;

        //rensa hela utkastet
        public async Task ClearAsync()
        {
            Draft = new TripDraft();
            await _localStorage.RemoveItemAsync(StorageKey);
        }

        //lägg till stopp i backend efter att resan sparats
        public async Task<bool> AddStopToTripAsync(int tripId, string mapServicePlaceId)
        {
            var tripStop = new TripStopFront
            {
                TripId = tripId,
                MapServicePlaceId = mapServicePlaceId
            };

            var response = await _http.PostAsJsonAsync("api/TripStop", tripStop);
            return response.IsSuccessStatusCode;
        }

        //hämta från-plats från localstorage för att skriva ut
        public async Task<PlaceDetailsFront?> GetFromPlaceDetailsAsync()
        {
            if (string.IsNullOrWhiteSpace(Draft.FromPlaceId))
            {
                return null;
            }

            try
            {
                return await _http.GetFromJsonAsync<PlaceDetailsFront>($"api/Place/{Draft.FromPlaceId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kunde inte hämta startplats: {ex.Message}");
                return null;
            }
        }

        //hämta till-plats från local storage för att skriva ut.
        public async Task<PlaceDetailsFront?> GetToPlaceDetailsAsync()
        {
            if (string.IsNullOrWhiteSpace(Draft.ToPlaceId))
            {
                return null;
            }

            try
            {
                return await _http.GetFromJsonAsync<PlaceDetailsFront>($"api/Place/{Draft.ToPlaceId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kunde inte hämta slutplats: {ex.Message}");
                return null;
            }
        }
    }
}