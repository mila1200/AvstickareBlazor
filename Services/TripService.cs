using AvstickareBlazor.Models;

namespace AvstickareBlazor
{
    public class TripService
    {
         public PlanTripRequest Input { get; set; } = new();
        public TripPlanResult? LatestTrip { get; set; }
    }
}

