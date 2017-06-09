using ELittoral.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace ELittoral.Services.Rest
{
    public class RESTFlightplanModelService
    {
        private string baseUri;
        private string namespaceUri = "flightplans/";
        private HttpClient httpClient;
        private CancellationTokenSource cts;

        public RESTFlightplanModelService(string uri)
        {
            httpClient = new HttpClient();
            cts = new CancellationTokenSource();
            baseUri = uri;
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
            HttpResponseMessage response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

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
