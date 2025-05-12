namespace AvstickareBlazor.Models
{
    public class TripPlanResult
    {
        public string? FromPlaceId { get; set; }
        public string? ToPlaceId { get; set; }
        public string? Polyline { get; set; }
        public string? Distance { get; set; }
        public string? Duration { get; set; }
        public List<PlaceToShow> SuggestedPlaces { get; set; } = [];
    }

    public class PlaceToShow
    {
        public string? Name { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string? MapServicePlaceId { get; set; }
    }
}