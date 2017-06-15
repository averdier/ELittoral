using ELittoral.Models;
using System.Collections.Generic;

namespace ELittoral.Services.Rest
{
    public class Gimbal
    {
        public double yaw { get; set; }
        public double pitch { get; set; }
        public double roll { get; set; }
    }

    public class GPSCoord
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public double alt { get; set; }
    }

    public class DroneParameters
    {
        public GPSCoord coord { get; set; }
        public double rotation { get; set; }
        public Gimbal gimbal { get; set; }
    }

    public class Waypoint
    {
        public object id { get; set; }
        public int number { get; set; }
        public DroneParameters parameters { get; set; }
    }

    public class Resource
    {
        public int id { get; set; }
        public int recon_id { get; set; }
        public string created_on { get; set; }
        public int number { get; set; }
        public object filename { get; set; }
        public DroneParameters parameters { get; set; }
    }

    public class Recon
    {
        public int id { get; set; }
        public int flightplan_id { get; set; }
        public string created_on { get; set; }
        public int resources_count { get; set; }
        public List<Resource> resources { get; set; }
    }

    public class Analysis
    {
        public int id { get; set; }
        public string created_on { get; set; }
        public object total { get; set; }
        public object current { get; set; }
        public string state { get; set; }
        public object message { get; set; }
        public object result { get; set; }
        public Recon minuend_recon { get; set; }
        public Recon subtrahend_recon { get; set; }
        public List<AnalysisResult> results { get; set; }
    }

    public class BuilderOptions
    {
        public GPSCoord coord1 { get; set; }
        public GPSCoord coord2 { get; set; }
        public double alt_start { get; set; }
        public double alt_end { get; set; }
        public double h_increment { get; set; }
        public double v_increment { get; set; }
        public double d_rotation { get; set; }
        public Gimbal d_gimbal { get; set; }
        public bool save { get; set; }
    }

    public class AnalysisResult
    {
        public int id;
        public string created_on;
        public object filename;
        public object result;
        public Resource subtrahend_resource;
        public Resource minuend_resource;
    }

    public class FlightPlan
    {
        public object id { get; set; }
        public string created_on { get; set; }
        public object updated_on { get; set; }
        public string name { get; set; }
        public bool builded { get; set; }
        public double distance { get; set; }
        public int waypoints_count { get; set; }
        public int recons_count { get; set; }
        public BuilderOptions builder_options { get; set; }
        public List<Waypoint> waypoints { get; set; }
        public List<Recon> recons { get; set; }
    }

    public class FlightPlanDataContainer
    {
        public List<FlightPlan> flightplans { get; set; }
    }

    public class WaypointDataContainer
    {
        public List<Waypoint> waypoints { get; set; }
    }

    public class AnalysisDataContainer
    {
        public List<Analysis> analysis { get; set; }
    }

    public class ReconDataContainer
    {
        public List<Recon> recons { get; set; }
    }
}