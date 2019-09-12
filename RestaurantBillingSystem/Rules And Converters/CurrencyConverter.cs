using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RestaurantBillingSystem
{
    [ValueConversion(typeof(string), typeof(Brush))]
    class CurrencyConverter : IValueConverter
    {
        private decimal min;
        private decimal max;

        public decimal Min { get => min; set => min = value; }
        public decimal Max { get => max; set => max = value; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal priceInput = 0;
            string[] priceArrayString;
            Min = 0.00m;
            Max = 200.00m;

            if (!decimal.TryParse(value.ToString(), out priceInput))
            {
                return Brushes.Red;
            }

            if (priceInput < Min || priceInput > Max)
            {
                return Brushes.Red;
            }

            if (priceInput.ToString().Contains('.'))
            {
                priceArrayString = priceInput.ToString().Split('.');
                //check if currency input does not have more than 2 decimals
                if (priceArrayString[1].Length > 2)
                {
                    return Brushes.Red;
                }
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
