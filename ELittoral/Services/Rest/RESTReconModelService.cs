using ELittoral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Services.Rest
{
    public class RESTReconModelService
    {
        public static ReconModel ReconToReconModel(Recon recon)
        {
            var model = new ReconModel
            {
                Id = recon.id,
                CreatedAt = recon.created_on,
                ResourceCount = recon.resources_count
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
    }
}
