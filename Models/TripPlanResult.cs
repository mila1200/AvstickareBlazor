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
}