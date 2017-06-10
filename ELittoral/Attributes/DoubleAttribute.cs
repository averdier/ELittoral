using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Attributes
{
    public class DoubleAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // The [Required] attribute should test this.
            if (value == null)
            {
                return true;
            }

            double result;
            return double.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }
    }
}
