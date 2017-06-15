using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace ELittoral.Models
{
    public class BuilderOptionsModel
    {
        public GimbalModel Gimbal { get; set; }

        public Geopoint BuildFrom { get; set; }

        public Geopoint BuildTo { get; set; }

        public double HorizontalIncrement { get; set; }

        public double VerticalIncrement { get; set; }

        public double Rotation { get; set; }

        public double StartAltitude { get; set; }

        public double EndAltitude { get; set; }
    }
}
