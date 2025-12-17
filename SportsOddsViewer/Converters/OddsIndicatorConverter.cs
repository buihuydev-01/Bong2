using System;
using System.Globalization;
using System.Windows.Data;

namespace SportsOddsViewer
{
    public class OddsIndicatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            
            var str = value.ToString();
            if (string.IsNullOrWhiteSpace(str)) return "";
            
            if (double.TryParse(str, out double num))
            {
                if (num < 0)
                    return "▼"; // Down arrow for negative
                else if (num > 0)
                    return "▲"; // Up arrow for positive
            }
            
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
