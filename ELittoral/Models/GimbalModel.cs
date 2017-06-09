using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    public class GimbalModel
    {
        private double _yaw = 0;
        public double Yaw
        {
            get { return _yaw; }
            set
            {
                if (value >= -180 && value <= 180)
                {
                    _yaw = value;
                }
            }
        }

        private double _pitch = 0;
        public double Pitch
        {
            get { return _pitch; }
            set
            {
                if (value >= -180 && value <= 180)
                {
                    _pitch = value;
                }
            }
        }

        private double _roll = 0;
        public double Roll
        {
            get { return _roll; }
            set
            {
                if (value >= -180 && value <= 180)
                {
                    _roll = value;
                }
            }
        }
    }
}
