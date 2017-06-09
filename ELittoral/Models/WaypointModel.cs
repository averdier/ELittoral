using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    public class WaypointModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public DroneParametersModel Parameters { get; set; }
    }
}
