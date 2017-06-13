using ELittoral.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace ELittoral.Services.Rest
{
    public class RESTResourceModelService
    {
        private string baseUri;
        private string namespaceUri = "resources/";

        private HttpBaseProtocolFilter filter;
        private HttpClient httpClient;
        private CancellationTokenSource cts;

        public static ReconResourceModel ResourceToReconResourceModel(Resource resource)
        {
            var model = new ReconResourceModel
            {
                Id = resource.id,
                CreatedAt = resource.created_on,
                Number = resource.number,
                Filename = (resource.filename != null) ? resource.filename.ToString() : null,
                Thumbnail = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/" + resource.id + "/thumbnail"),
                Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/" + resource.id + "/content")
            };
            return model;
        }

        public RESTResourceModelService(string uri)
        {
            filter = new HttpBaseProtocolFilter();
            httpClient = new HttpClient(filter);
            cts = new CancellationTokenSource();
            baseUri = uri;
        }

        public async Task<ReconResourceModel> PostReconResource(int reconId, int number, DroneParametersModel parameters)
        {
            await Task.CompletedTask;

            Uri resourceUri = new Uri(baseUri + namespaceUri);

            var post_options = new
            {
                recon_id = reconId,
                number = number,
                parameters = new DroneParameters
                {
                    gimbal = new Gimbal
                    {
                        yaw = parameters.Gimbal.Yaw,
                        pitch = parameters.Gimbal.Pitch,
                        roll = parameters.Gimbal.Yaw
                    },
                    coord = new GPSCoord
                    {
                        lat = parameters.Coord.Position.Latitude,
                        lon = parameters.Coord.Position.Longitude,
                        alt = parameters.Coord.Position.Altitude
                    },
                    rotation = parameters.Rotation
                },
            };


            string jsonObject = "";
            jsonObject = JsonConvert.SerializeObject(post_options);

            var httpC = new System.Net.Http.HttpClient();
            var response = await httpC.PostAsync(resourceUri, new System.Net.Http.StringContent(jsonObject, System.Text.Encoding.UTF8, "application/json"));

            var strResponse = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<Resource>(strResponse);

            return ResourceToReconResourceModel(resource);
        }

        public async Task<bool> PostResourceContentAsync(int resourceId, StorageFile file)
        {
            var nHttpClient = new System.Net.Http.HttpClient();
            nHttpClient.BaseAddress = new Uri("http://vps361908.ovh.net/dev/elittoral/api/");
            System.Net.Http.MultipartFormDataContent form = new System.Net.Http.MultipartFormDataContent();

            System.Net.Http.HttpContent content = new System.Net.Http.StringContent("files");
            form.Add(content, "files");

            var stream = await file.OpenStreamForReadAsync();
            content = new System.Net.Http.StreamContent(stream);
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = file.Name
            };
            form.Add(content);

            var response = await nHttpClient.PostAsync(namespaceUri + resourceId + "/content", form);
            var strResponse = await response.Content.ReadAsStringAsync();

            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
