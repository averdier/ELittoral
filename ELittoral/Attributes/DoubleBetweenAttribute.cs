using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Attributes
{
    public class DoubleBetweenAttribute : ValidationAttribute
    {
        double min;
        double max;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public DoubleBetweenAttribute(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            double result;
            if (double.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out result))
            {
                if (result >= min && result <= max)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
