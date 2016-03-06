using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TestImageViewer.Converters
{
    /// <summary>
    /// Calculates <see cref="ListViewItem"/> width depending on the current <see cref="Window"/> size
    /// </summary>
    public class ListViewSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return ((double)value - 40) / 5;
            }
            return 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
