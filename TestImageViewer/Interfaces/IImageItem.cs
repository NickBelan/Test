using System;
using System.Windows.Media.Imaging;

namespace TestImageViewer.Interfaces
{
    public interface IImageItem
    {
        string FilePath { get; }

        BitmapImage Image { get; }

        Guid Id { get; }

        IImageItem Update(IImageItem updatedItem, string newFilePath);

    }
}
