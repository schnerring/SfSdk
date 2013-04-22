using System;
using System.Windows.Data;

namespace SfBot.Converters
{
    public class DamageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (values.Length != 2) throw new NotImplementedException();
            return values[0] + " - " + values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}