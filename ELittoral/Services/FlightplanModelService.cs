using ELittoral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Services
{
    public class FlightplanModelService
    {
        public async Task<IEnumerable<FlightplanModel>> GetDataAsync()
        {
            await Task.CompletedTask;
            var data = new List<FlightplanModel>();

            var flightplan = new FlightplanModel {
                Name = "Plan de vol 1",
                CreatedAt = "07/08/2017",
                Recons = new List<ReconModel>()
            };

            flightplan.Recons.Add(new ReconModel { CreatedAt = "05/06/2017" });
            flightplan.Recons.Add(new ReconModel { CreatedAt = "06/06/2017" });

            data.Add(flightplan);

            flightplan = new FlightplanModel
            {
                Name = "Plan de vol 2",
                CreatedAt = "07/08/2017",
                Recons = new List<ReconModel>()
            };

            flightplan.Recons.Add(new ReconModel { CreatedAt = "05/06/2016" });
            flightplan.Recons.Add(new ReconModel { CreatedAt = "10/08/2016" });

            data.Add(flightplan);

            return data;
        }
    }
}
