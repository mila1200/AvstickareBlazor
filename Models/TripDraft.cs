namespace AvstickareBlazor.Models
{
    public class TripDraft
    {
        public string? FromPlaceId { get; set; }
        public string? ToPlaceId { get; set; }
        public List<string> StopPlaceIds { get; set; } = [];
    }
}