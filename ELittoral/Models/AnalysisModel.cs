using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    public class AnalysisModel
    {
        public int Id;

        public string CreatedAt { get; set; }

        public string State { get; set; }

        public string Message { get; set; }

        public string Current { get; set; }

        public string Total { get; set; }

        public string Progression { get { return Current + "/" + Total;  } }

        public string Result { get; set; }

        public IList<AnalysisResultModel> Results { get; set; }
    }
}
