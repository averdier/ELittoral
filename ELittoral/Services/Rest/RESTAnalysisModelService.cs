using ELittoral.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace ELittoral.Services.Rest
{
    public class RESTAnalysisModelService
    {
        private string baseUri;
        private string namespaceUri = "analysis/";

        private HttpBaseProtocolFilter filter;
        private HttpClient httpClient;
        private CancellationTokenSource cts;

        public RESTAnalysisModelService(string uri)
        {
            filter = new HttpBaseProtocolFilter();
            httpClient = new HttpClient(filter);
            cts = new CancellationTokenSource();
            baseUri = uri;
        }

        public static AnalysisResultModel AnalysisResultToAnalysisResultModel(AnalysisResult result)
        {
            var model = new AnalysisResultModel
            {
                Id = result.id,
                Filename = (result.filename != null) ? result.filename.ToString() : null,
                Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/results/" + result.id + "/content")
            };

            if (result.result != null)
            {
                model.Result = result.result.ToString();
            }
            

            if (result.minuend_resource != null)
            {
                model.MinuendRessource = RESTResourceModelService.ResourceToReconResourceModel(result.minuend_resource);
            }

            if (result.subtrahend_resource != null)
            {
                model.SubtrahendRessource = RESTResourceModelService.ResourceToReconResourceModel(result.subtrahend_resource);
            }

            return model;
        }

        public static AnalysisModel AnalysisToAnalysisModel(Analysis analysis)
        {
            var model = new AnalysisModel
            {
                Id = analysis.id,
                CreatedAt = analysis.created_on,
                State = analysis.state,
                Message = (analysis.message != null) ? analysis.ToString() : null,
            };

            if (analysis.total != null)
            {
                model.Total = analysis.total.ToString();
            }

            if (analysis.current != null)
            {
                model.Current = analysis.current.ToString();
            }

            if (analysis.result != null)
            {
                model.Result = analysis.result.ToString();
            }

            if (analysis.results != null)
            {
                model.Results = new List<AnalysisResultModel>();

                foreach(AnalysisResult anr in analysis.results)
                {
                    model.Results.Add(AnalysisResultToAnalysisResultModel(anr));
                }
            }

            return model;
        }

        public async Task<IEnumerable<AnalysisModel>> GetAnalysesAsync()
        {
            await Task.CompletedTask;

            Uri resourceUri = new Uri(baseUri + namespaceUri);
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;

            HttpResponseMessage response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

            var strResponse = await response.Content.ReadAsStringAsync();
            var container = JsonConvert.DeserializeObject<AnalysisDataContainer>(strResponse);

            var data = new List<AnalysisModel>();

            if (container.analysis != null)
            {
                foreach (Analysis an in container.analysis)
                {
                    data.Add(AnalysisToAnalysisModel(an));
                }
            }

            return data;
        }

        public async Task<AnalysisModel> GetAnalysisFromIdAsync(int analysisId)
        {
            await Task.CompletedTask;
            Uri resourceUri = new Uri(baseUri + namespaceUri + analysisId);
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;

            HttpResponseMessage response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

            var strResponse = await response.Content.ReadAsStringAsync();
            var analysis = JsonConvert.DeserializeObject<Analysis>(strResponse);

            return AnalysisToAnalysisModel(analysis);
        }

        public async Task<AnalysisModel> LaunchAnalysis(ReconModel minuend, ReconModel subtrahend)
        {
            await Task.CompletedTask;

            Uri resourceUri = new Uri(baseUri + namespaceUri);

            var post_options = new
            {
                minuend_recon_id = minuend.Id,
                subtrahend_recon_id = subtrahend.Id
            };


            string jsonObject = "";
            jsonObject = JsonConvert.SerializeObject(post_options);

            var httpC = new System.Net.Http.HttpClient();
            var response = await httpC.PostAsync(resourceUri, new System.Net.Http.StringContent(jsonObject, System.Text.Encoding.UTF8, "application/json"));

            var strResponse = await response.Content.ReadAsStringAsync();
            var analysis = JsonConvert.DeserializeObject<Analysis>(strResponse);

            return AnalysisToAnalysisModel(analysis);
        }

        public async Task<bool> DeleteAnalysisFromIdAsync(int analysisId)
        {
            await Task.CompletedTask;
            Uri resourceUri = new Uri(baseUri + namespaceUri + analysisId);

            var response = await httpClient.DeleteAsync(resourceUri).AsTask(cts.Token);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public void CancelTask()
        {
            cts.Cancel();
            cts.Dispose();

            cts = new CancellationTokenSource();
        }
    }

}
