using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Attributes
{
    public class DoubleUpperZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // The [Required] attribute should test this.
            if (value == null)
            {
                return true;
            }

            double result;
            if (double.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out result))
            {
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
