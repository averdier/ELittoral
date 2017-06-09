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

        public double Distance { get; set; }

        public int WaypointCount { get; set; }

        public int ReconCount { get; set; }

        public IList<WaypointModel> Waypoints { get; set; }

        public IList<ReconModel> Recons { get; set; }
    }
}
