using ELittoral.Models;
using System.Collections.Generic;

namespace ELittoral.Services.Rest
{
    public class FlightPlan
    {
        public object id { get; set; }
        public string created_on { get; set; }
        public object updated_on { get; set; }
        public string name { get; set; }
        public double distance { get; set; }
        public int waypoints_count { get; set; }
        public int recons_count { get; set; }
    }

    public class FlightPlanDataContainer
    {
        public List<FlightPlan> flightplans { get; set; }
    }
}