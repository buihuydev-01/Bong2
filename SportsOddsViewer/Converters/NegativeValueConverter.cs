using System;
using System.Globalization;
using System.Windows.Data;

namespace SportsOddsViewer
{
    public class NegativeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            
            var str = value.ToString();
            if (string.IsNullOrWhiteSpace(str)) return false;
            
            if (double.TryParse(str, out double num))
            {
                return num < 0;
            }
            
            return str.StartsWith("-");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
