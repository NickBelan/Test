using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using ImageBlurFilter;

namespace TestImageViewer.Helpers
{
    /// <summary>
    /// Applies blur filter to the Image
    /// Implementation is a quite ugly because there is no simple way to convert <see cref="Bitmap"/> to <see cref="BitmapImage"/> in memory.
    /// </summary>
    public static class BlurFilterHelper
    {
        const ExtBitmap.BlurType blurType = ExtBitmap.BlurType.MotionBlur9x9;
        const string blurredFilePostfix = "(blur)";
        const string symbolToExtendPath = "-";

        public static string ApplyBlur(BitmapImage image, string filePath)
        {
            string extension = Path.GetExtension(filePath);
            string newPath = GetNewPathForModifiedImage(filePath, extension, false);
            while (File.Exists(newPath))
            {
                newPath = GetNewPathForModifiedImage(filePath, extension, true);
            }

            Bitmap source = ConvertBitmapImage2Bitmap(image);
            Bitmap result = source.ImageBlurFilter(blurType);

            SaveModifiedBitmap(result, newPath, extension);
            return newPath; 
        }

        #region Helpers

        private static void SaveModifiedBitmap(Bitmap source, string newPath, string extension)
        {
            ImageFormat imgFormat = GetImageFormatFromExtension(extension);
            using (StreamWriter streamWriter = new StreamWriter(newPath, false))
            {
                source.Save(streamWriter.BaseStream, imgFormat);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        private static Bitmap ConvertBitmapImage2Bitmap(BitmapImage bitmapImage)
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

        private static string GetNewPathForModifiedImage(string oldPath, string extension, bool addSymbolToPath)
        {
            return string.Concat(Path.GetDirectoryName(oldPath), @"\",
                Path.GetFileNameWithoutExtension(oldPath), 
                oldPath.Contains(blurredFilePostfix) ? String.Empty : blurredFilePostfix,
                addSymbolToPath ? symbolToExtendPath : String.Empty, 
                extension);
        }

        private static ImageFormat GetImageFormatFromExtension(string extension)
        {
            switch (extension.ToUpper())
            {
                case ".BMP":
                    return ImageFormat.Bmp;
                case ".JPG":
                    return ImageFormat.Jpeg;
                case ".JPEG":
                    return ImageFormat.Jpeg;
                case ".PNG":
                    return ImageFormat.Png;
                case ".GIF":
                    return ImageFormat.Gif;
                case ".TIF":
                    return ImageFormat.Tiff;
                case ".TIFF":
                    return ImageFormat.Tiff;
            }

            throw new ArgumentOutOfRangeException();
        }

        #endregion Helpers
    }
}
