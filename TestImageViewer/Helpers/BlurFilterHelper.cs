using System.Drawing;
using System.Windows.Media.Imaging;
using ImageBlurFilter;

namespace TestImageViewer.Helpers
{
    /// <summary>
    /// Applies blur filter to the Image
    /// </summary>
    public static class BlurFilterHelper
    {
        const ExtBitmap.BlurType blurType = ExtBitmap.BlurType.GaussianBlur5x5;

        public static BitmapImage ApplyBlur(BitmapImage image)
        {
            Bitmap source = BitmapHelper.ConvertBitmapImage2Bitmap(image);
            Bitmap result = source.ImageBlurFilter(blurType);

            return BitmapHelper.ConvertBitmap2BitmapImage(result); 
        }
    }
}
