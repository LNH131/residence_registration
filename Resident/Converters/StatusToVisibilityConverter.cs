using Resident.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Resident.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If the registration status is "Pending", show the buttons.
            if (value is string status && status == Status.Pending.ToString())
            {
                return Visibility.Visible;
            }
            // Otherwise, hide them.
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
