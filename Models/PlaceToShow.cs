
namespace AvstickareBlazor.Models
{
    //för att visa resmål på kartan
    public class PlaceToShow
    {
        public string? Name { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string? MapServicePlaceId { get; set; }
    }
}