using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Resident.Converters
{
    public class MessageAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // e.g. Return a HorizontalAlignment based on fromUserId
            return HorizontalAlignment.Left; // Just an example
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
