using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using TestImageViewer.Interfaces;

namespace TestImageViewer.Converters
{
    /// <summary>
    /// Returns <see cref="Image"/> from the provided <see cref="IImageItem"/>
    /// </summary>
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            IImageItem imageItem = value as IImageItem;
            if (imageItem != null)
            {
                return imageItem.Image;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
