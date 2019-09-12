using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RestaurantBillingSystem
{
    class CurrencyRule : ValidationRule
    {
        private decimal min;
        private decimal max;

        public decimal Min { get => min; set => min = value; }
        public decimal Max { get => max; set => max = value; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal priceInput = 0;
            string[] priceArrayString;

            if (!decimal.TryParse(value.ToString(), out priceInput))
            {
                if (value.ToString() == string.Empty)
                {
                    //dont want blank form after clearing to show red validation error msgs
                    return new ValidationResult(false, "Cannot be blank");
                }

                return new ValidationResult(false, "Not a valid currency amount");
            }

            if (priceInput < Min || priceInput > Max)
            {
                return new ValidationResult(false, $"Must be between ${Min} - ${Max}");
            }

            if (priceInput.ToString().Contains('.'))
            {
                priceArrayString = priceInput.ToString().Split('.');
                //check if currency input does not have more than 2 decimals
                if (priceArrayString[1].Length > 2)
                {
                    return new ValidationResult(false, "Invalid currency decimal places");
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
