using System;
using System.Windows.Media.Imaging;

namespace TestImageViewer.Interfaces
{
    public interface IImageItem
    {
        string FilePath { get; }

        bool IsBlurred { get; set; }

        bool HasBlurredImage { get; }

        BitmapImage Image { get; }

        BitmapSource FullImage { get; }

        Guid Id { get; }

        IImageItem Update(BitmapImage updatedImage);

    }
}
