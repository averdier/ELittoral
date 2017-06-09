using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace ELittoral.Models
{
    public class DroneParametersModel
    {
        public GimbalModel Gimbal { get; set; }

        public Geopoint Coord { get; set; }

        private double _rotation = 0;
        public double Rotation
        {
            get { return _rotation; }
            set
            {
                if (value >= -180 && value <= 180)
                {
                    _rotation = value;
                }
            }
        }
    }
}
