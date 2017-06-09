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
        private HttpClient httpClient;
        private CancellationTokenSource cts;

        public RESTFlightplanModelService(string uri)
        {
            httpClient = new HttpClient();
            cts = new CancellationTokenSource();
            baseUri = uri;
        }

        public async Task<IEnumerable<FlightplanModel>> GetFlightplansAsync()
        {
            await Task.CompletedTask;

            Uri resourceUri = new Uri(baseUri);
            HttpResponseMessage response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

            var strResponse = await response.Content.ReadAsStringAsync();
            var container = JsonConvert.DeserializeObject<FlightPlanDataContainer>(strResponse);

            var data = new List<FlightplanModel>();

            if (container.flightplans != null)
            {
                foreach (FlightPlan fp in container.flightplans)
                {
                    data.Add(new FlightplanModel
                    {
                        Id = Convert.ToInt32(fp.id),
                        Name = fp.name,
                        CreatedAt = fp.created_on,
                        Distance = string.Format("{0} m", Math.Round(fp.distance, 4).ToString(CultureInfo.InvariantCulture))
                    });
                }
            }

            return data;
        }

        public void CancelTask()
        {
            cts.Cancel();
            cts.Dispose();

            cts = new CancellationTokenSource();
        }
    }
}
