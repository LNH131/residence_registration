using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Resident.Converters
{
    public class StatusToVisibilityConverterExtension : MarkupExtension, IValueConverter
    {
        // Use a static instance so that the same converter is reused.
        private static StatusToVisibilityConverterExtension _instance;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new StatusToVisibilityConverterExtension());
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Assume value is a string representing the status.
            if (value is string status)
            {
                // Show the button only if status equals "Pending" (ignoring case)
                return status.Equals("Pending", StringComparison.OrdinalIgnoreCase) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
