using Microsoft.UI.Xaml.Data;

namespace DashCAN.Converters
{
    public class WarningTypeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var warningType = WarningType.None;
            if (value is WarningType) warningType = (WarningType)value;

            var paramType = WarningType.None;
            if (parameter is string) Enum.TryParse((string)parameter, out paramType);
            if (paramType == WarningType.None) return Visibility.Collapsed;

            return warningType == paramType ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
