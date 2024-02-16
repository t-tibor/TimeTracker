using System.Globalization;
using System.Windows.Data;

namespace TimeTracker.Converters
{
    internal class DateOnly2DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateOnly day)
            {
                return day.ToDateTime(TimeOnly.MinValue);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime date)
            {
                return DateOnly.FromDateTime(date);
            }
            else
            {
                return null;
            }
        }
    }
}
