using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    public class ImageModel
    {
        public Uri Thumbnail { get; set; }

        public Uri Content { get; set; }

        public string CreatedAt { get; set; }
    }
}
