using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    public class FlightplanModel
    {
        public string Name { get; set; }

        public IList<ReconModel> Recons { get; set; }
    }
}
