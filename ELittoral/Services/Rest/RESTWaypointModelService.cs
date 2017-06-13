using ELittoral.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace ELittoral.Services.Rest
{
    public class RESTWaypointModelService
    {
        private string baseUri;
        private string namespaceUri = "waypoints/";

        private HttpBaseProtocolFilter filter;
        private HttpClient httpClient;
        private CancellationTokenSource cts;

        public static GimbalModel GimbalToGimbalModel(Gimbal gimbal)
        {
            var model = new GimbalModel
            {
                Yaw = gimbal.yaw,
                Pitch = gimbal.pitch,
                Roll = gimbal.roll
            };

            return model;
        }

        public static DroneParametersModel DroneParmetersToDroneParametersModel(DroneParameters parameters)
        {
            var model = new DroneParametersModel
            {
                Gimbal = GimbalToGimbalModel(parameters.gimbal),
                Rotation = parameters.rotation,
                Coord = new Geopoint(new BasicGeoposition
                {
                    Latitude = parameters.coord.lat,
                    Longitude = parameters.coord.lon,
                    Altitude = parameters.coord.alt
                })
            };

            return model;
        }
 
        public static WaypointModel WaypointToWaypointModel(Waypoint waypoint)
        {
            var model = new WaypointModel
            {
                Number = waypoint.number,
                Parameters = DroneParmetersToDroneParametersModel(waypoint.parameters)
            };

            if (waypoint.id != null)
            {
                model.Id = Convert.ToInt32(waypoint.id);
            }

            return model;
        }

        public RESTWaypointModelService(string uri)
        {
            filter = new HttpBaseProtocolFilter();
            httpClient = new HttpClient(filter);
            cts = new CancellationTokenSource();
            baseUri = uri;
        }

        public async Task<List<WaypointModel>> GetWaypointFromFlightplanIdAsync(int flightplanId)
        {
            await Task.CompletedTask;

            Uri resourceUri = new Uri(baseUri + namespaceUri + "?flightplan_id=" + flightplanId);
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;

            HttpResponseMessage response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

            var strResponse = await response.Content.ReadAsStringAsync();
            var container = JsonConvert.DeserializeObject<WaypointDataContainer>(strResponse);

            var data = new List<WaypointModel>();

            if (container.waypoints != null)
            {
                foreach (Waypoint wp in container.waypoints)
                {
                    data.Add(WaypointToWaypointModel(wp));
                }
            }

            return data;
        }
    }
}
