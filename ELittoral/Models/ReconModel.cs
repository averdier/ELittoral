using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    public class ReconModel
    {
        public int Id { get; set; }
        public int FlightplanId { get; set; }
        public string CreatedAt { get; set; }
        public int ResourceCount { get; set; }
        public IList<ReconResourceModel> Resources { get; set; }
    }
}
