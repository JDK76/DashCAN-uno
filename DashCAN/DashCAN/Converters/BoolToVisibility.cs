using Microsoft.UI.Xaml.Data;

namespace DashCAN.Converters
{
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = false;
            if (value is bool) boolValue = (bool)value;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
