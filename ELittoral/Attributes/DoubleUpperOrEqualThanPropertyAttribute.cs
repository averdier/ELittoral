using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DoubleUpperOrEqualThanPropertyAttribute : ValidationAttribute
    {
        string propertyName;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="propertyName"></param>
        public DoubleUpperOrEqualThanPropertyAttribute(string propertyName)
        {
            this.propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var propertyInfo = validationContext.ObjectInstance.GetType().GetProperty(this.propertyName);

            if (propertyInfo == null)
            {
                // throw exception.
                return ValidationResult.Success;
            }

            dynamic otherValue = propertyInfo.GetValue(validationContext.ObjectInstance);

            if (otherValue == null)
            {
                return ValidationResult.Success;
            }

            double dOv = 0;
            double dV = 0;

            double.TryParse(otherValue, NumberStyles.Float, CultureInfo.InvariantCulture, out dOv);
            double.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out dV);

            var compare = dOv - dV;

            if (compare > 0)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
