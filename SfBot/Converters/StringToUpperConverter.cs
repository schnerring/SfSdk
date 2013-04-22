using System;
using System.Globalization;
using System.Windows.Data;

namespace SfBot.Converters
{
    public class StringToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (value is DateTime) return ((DateTime) value).ToString(CultureInfo.InvariantCulture).ToUpper();
            if (value is int) return ((int) value).ToString(CultureInfo.InvariantCulture).ToUpper();
            var s = value as string;
            if (s != null) return s.ToUpper();

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}