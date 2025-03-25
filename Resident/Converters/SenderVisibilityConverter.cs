using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Resident.Converters
{
    public class SenderVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Example: Hide or show based on matching user IDs
            if (value is int fromUserId && int.TryParse(parameter?.ToString(), out int currentUserId))
            {
                return fromUserId == currentUserId ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
