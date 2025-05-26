namespace AvstickareBlazor.Models
{
    public class Trip
    {   
        public int TripId { get; set; }
        public string? FromPlaceId { get; set; }
        public string? ToPlaceId { get; set; }

        public string Name { get; set; } = $"Min resa {DateTime.UtcNow:yyyy-MM-dd}";
    }
}