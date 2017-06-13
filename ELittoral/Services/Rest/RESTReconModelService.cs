using ELittoral.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace ELittoral.Services.Rest
{
    public class RESTReconModelService
    {
        private string baseUri;
        private string namespaceUri = "recons/";

        private HttpBaseProtocolFilter filter;
        private HttpClient httpClient;
        private CancellationTokenSource cts;

        public RESTReconModelService(string uri)
        {
            filter = new HttpBaseProtocolFilter();
            httpClient = new HttpClient(filter);
            cts = new CancellationTokenSource();
            baseUri = uri;
        }

        public static ReconModel ReconToReconModel(Recon recon)
        {
            var model = new ReconModel
            {
                Id = recon.id,
                CreatedAt = recon.created_on,
                ResourceCount = recon.resources_count,
            };

            if (recon.resources != null)
            {
                model.Resources = new List<ReconResourceModel>();
                foreach(Resource res in recon.resources)
                {
                    model.Resources.Add(RESTResourceModelService.ResourceToReconResourceModel(res));
                }
            }

            return model;
        }

        public async Task<IEnumerable<ReconModel>> GetReconsFromFlightplanIdAsync(int flightplanId)
        {
            await Task.CompletedTask;

            Uri resourceUri = new Uri(baseUri + namespaceUri + "?flightplan_id=" + flightplanId);
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;

            HttpResponseMessage response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

            var strResponse = await response.Content.ReadAsStringAsync();
            var container = JsonConvert.DeserializeObject<ReconDataContainer>(strResponse);

            var data = new List<ReconModel>();

            if (container.recons != null)
            {
                foreach (Recon rc in container.recons)
                {
                    data.Add(ReconToReconModel(rc));
                }
            }

            return data;
        }

        public async Task<ReconModel> GetReconFromIdAsync(int reconId)
        {
            await Task.CompletedTask;
            Uri resourceUri = new Uri(baseUri + namespaceUri + reconId);
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;

            HttpResponseMessage response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

            var strResponse = await response.Content.ReadAsStringAsync();
            var recon = JsonConvert.DeserializeObject<Recon>(strResponse);

            return ReconToReconModel(recon);
        }

        public async Task<bool> DeleteReconFromIdAsync(int reconId)
        {
            await Task.CompletedTask;
            Uri resourceUri = new Uri(baseUri + namespaceUri + reconId);

            var response = await httpClient.DeleteAsync(resourceUri).AsTask(cts.Token);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<ReconModel> PostRecon(int flightplanId)
        {
            await Task.CompletedTask;

            Uri resourceUri = new Uri(baseUri + namespaceUri);

            var post_options = new
            {
                flightplan_id = flightplanId
            };


            string jsonObject = "";
            jsonObject = JsonConvert.SerializeObject(post_options);

            var httpC = new System.Net.Http.HttpClient();
            var response = await httpC.PostAsync(resourceUri, new System.Net.Http.StringContent(jsonObject, System.Text.Encoding.UTF8, "application/json"));

            var strResponse = await response.Content.ReadAsStringAsync();
            var recon = JsonConvert.DeserializeObject<Recon>(strResponse);

            return ReconToReconModel(recon);
        }
    }
}
