using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantBillingSystem
{
    class NameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            char[] arrayString = ((string)value).ToCharArray();
            foreach (char c in arrayString)
            {
                if (char.IsDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c))
                {
                    return new ValidationResult(false, "Can't have digits, symbols, and punctuations");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
