using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace TestImageViewer.Helpers
{
    /// <summary>
    /// Provides methods for Bitmap processing
    /// </summary>
    public static class BitmapHelper
    {
        public static Bitmap ConvertBitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }

        public static BitmapImage ConvertBitmap2BitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        public static Image GetThumbnailImage(Image image, int width, int height)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;

            float nPercentW = (width / (float)sourceWidth);
            float nPercentH = (height / (float)sourceHeight);
            float nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            return image.GetThumbnailImage(destWidth, destHeight, null, (IntPtr) 0);
        }

        public static Image ReadImageWithOrientation(string fileName)
        {
            Image originalImage = Image.FromFile(fileName);
            if (originalImage.PropertyIdList.Contains(0x0112))
            {
                int rotationValue = originalImage.GetPropertyItem(0x0112).Value[0];
                switch (rotationValue)
                {
                    case 1: // landscape
                        break;

                    case 8: // 90 right
                        originalImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;

                    case 3: // bottom up
                        originalImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;

                    case 6: // 90 left
                        originalImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                }
            }
            return originalImage;
        }
    }
}
