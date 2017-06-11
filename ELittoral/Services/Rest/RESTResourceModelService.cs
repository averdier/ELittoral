using ELittoral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Services.Rest
{
    public class RESTResourceModelService
    {
        public static ReconResourceModel ResourceToReconResourceModel(Resource resource)
        {
            var model = new ReconResourceModel
            {
                Id = resource.id,
                CreatedAt = resource.created_on,
                Number = resource.number,
                Thumbnail = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/" + resource.id + "/thumbnail"),
                Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/" + resource.id + "/content")
            };

            return model;
        }
    }
}
