using ELittoral.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace ELittoral.Services.Rest
{
    public class RESTFlightplanModelService
    {
        private string baseUri;
        private string namespaceUri = "flightplans/";

        private HttpBaseProtocolFilter filter;
        private HttpClient httpClient;
        private CancellationTokenSource cts;

        public RESTFlightplanModelService(string uri)
        {
            filter = new HttpBaseProtocolFilter();
            httpClient = new HttpClient(filter);
            cts = new CancellationTokenSource();
            baseUri = uri;
        }

        public static BuilderOptionsModel BuilderOptionsToBuilderOptionsModel(BuilderOptions options)
        {
            BuilderOptionsModel model = new BuilderOptionsModel
            {
                Gimbal = RESTWaypointModelService.GimbalToGimbalModel(options.d_gimbal),
                Rotation = options.d_rotation,
                StartAltitude = options.alt_start,
                EndAltitude = options.alt_end,
                BuildFrom = new Geopoint(new BasicGeoposition
                {
                    Latitude = options.coord1.lat,
                    Longitude = options.coord1.lon,
                    Altitude = options.coord1.alt
                }),
                BuildTo = new Geopoint(new BasicGeoposition
                {
                    Latitude = options.coord2.lat,
                    Longitude = options.coord2.lon,
                    Altitude = options.coord2.alt
                })

            };

            return model;
        }

        public static FlightplanModel FlightplanToFlightplanModel(FlightPlan flightplan)
        {
            var model = new FlightplanModel
            {
                Name = flightplan.name,
                CreatedAt = flightplan.created_on,
                Distance = flightplan.distance,
                WaypointCount = flightplan.waypoints_count,
                ReconCount = flightplan.recons_count
            };

            if (flightplan.id != null)
            {
                model.Id = Convert.ToInt32(flightplan.id);
            }

            if (flightplan.waypoints != null)
            {
                model.Waypoints = new List<WaypointModel>();
                foreach(Waypoint wp in flightplan.waypoints)
                {
                    model.Waypoints.Add(RESTWaypointModelService.WaypointToWaypointModel(wp));
                }
            }

            if (flightplan.builder_options != null)
            {
                model.Options = BuilderOptionsToBuilderOptionsModel(flightplan.builder_options);
            }

            if (flightplan.recons != null)
            {
                model.Recons = new List<ReconModel>();
                foreach(Recon rec in flightplan.recons)
                {
                    model.Recons.Add(RESTReconModelService.ReconToReconModel(rec));
                }
            }

            

            return model;
        }

        public async Task<IEnumerable<FlightplanModel>> GetFlightplansAsync()
        {
            await Task.CompletedTask;

            Uri resourceUri = new Uri(baseUri + namespaceUri);
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;

            HttpResponseMessage response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);
            
            var strResponse = await response.Content.ReadAsStringAsync();
            var container = JsonConvert.DeserializeObject<FlightPlanDataContainer>(strResponse);

            var data = new List<FlightplanModel>();

            if (container.flightplans != null)
            {
                foreach (FlightPlan fp in container.flightplans)
                {
                    data.Add(FlightplanToFlightplanModel(fp));
                }
            }

            return data;
        }

        public async Task<FlightplanModel> GetFlightplanFromIdAsync(int flightplanId)
        {
            await Task.CompletedTask;
            Uri resourceUri = new Uri(baseUri + namespaceUri + flightplanId);
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;

            HttpResponseMessage response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

            var strResponse = await response.Content.ReadAsStringAsync();
            var flightplan = JsonConvert.DeserializeObject<FlightPlan>(strResponse);

            return FlightplanToFlightplanModel(flightplan);
        }

        public async Task<bool> DeleteFlightplanFromIdAsync(FlightplanModel flightplan)
        {
            await Task.CompletedTask;
            Uri resourceUri = new Uri(baseUri + namespaceUri + flightplan.Id);

            var response = await httpClient.DeleteAsync(resourceUri).AsTask(cts.Token);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<FlightplanModel> BuildFlightplan(BuildOptionsModel options)
        {
            await Task.CompletedTask;

            Uri resourceUri = new Uri(baseUri + namespaceUri + "build");

            double lat1, lat2, lon1, lon2, yaw, pitch, roll, rotation, alt_end, alt_start, h_increment, v_increment = 0;

            double.TryParse(options.BuildFromLatitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lat1);
            double.TryParse(options.BuildToLatitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lat2);
            double.TryParse(options.BuildFromLongitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lon1);
            double.TryParse(options.BuildToLongitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lon2);
            double.TryParse(options.GimbalYaw, NumberStyles.Float, CultureInfo.InvariantCulture, out yaw);
            double.TryParse(options.GimbalPitch, NumberStyles.Float, CultureInfo.InvariantCulture, out pitch);
            double.TryParse(options.GimbalRoll, NumberStyles.Float, CultureInfo.InvariantCulture, out roll);
            double.TryParse(options.Rotation, NumberStyles.Float, CultureInfo.InvariantCulture, out rotation);
            double.TryParse(options.StartAltitude, NumberStyles.Float, CultureInfo.InvariantCulture, out alt_start);
            double.TryParse(options.EndAltitude, NumberStyles.Float, CultureInfo.InvariantCulture, out alt_end);
            double.TryParse(options.HorizontalIncrement, NumberStyles.Float, CultureInfo.InvariantCulture, out h_increment);
            double.TryParse(options.VerticalIncrement, NumberStyles.Float, CultureInfo.InvariantCulture, out v_increment);

            var post_options = new
            {
                save = true,
                flightplan_name = options.FlightPlanName,
                coord1 = new GPSCoord
                {
                    lat = lat1,
                    lon = lon1
                },
                coord2 = new GPSCoord
                {
                    lat = lat2,
                    lon = lon2
                },
                d_gimbal = new Gimbal
                {
                    yaw = yaw,
                    pitch = pitch,
                    roll = roll
                },
                alt_start = alt_start,
                alt_end = alt_end,
                h_increment = h_increment,
                v_increment = v_increment,
                d_rotation = rotation
            };

            string jsonObject = "";
            jsonObject = JsonConvert.SerializeObject(post_options);

            var httpC = new System.Net.Http.HttpClient();
            var response = await httpC.PostAsync(resourceUri, new System.Net.Http.StringContent(jsonObject, System.Text.Encoding.UTF8, "application/json"));

            var strResponse = await response.Content.ReadAsStringAsync();
            var flightplan = JsonConvert.DeserializeObject<FlightPlan>(strResponse);

            return FlightplanToFlightplanModel(flightplan);
        }

        public void CancelTask()
        {
            cts.Cancel();
            cts.Dispose();

            cts = new CancellationTokenSource();
        }
    }
}
