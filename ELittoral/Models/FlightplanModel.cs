using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    public class FlightplanModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public string Distance { get; set; }

        public IList<ReconModel> Recons { get; set; }
    }
}
