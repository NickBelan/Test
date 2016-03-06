using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestImageViewer.Converters
{
    /// <summary>
    /// Converts boolean values to <see cref="Visibility"/> depending on boolean value from parameter
    /// </summary>
    class DependentBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            bool parameterValue;
            if (value is bool && Boolean.TryParse(parameter.ToString(), out parameterValue))
            {
                if ((bool)value == parameterValue)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
