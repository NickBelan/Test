using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace TestImageViewer.Interfaces
{
    public interface IImagesModel
    {
        ObservableCollection<IImageItem> ImageItems { get; }
        
        IImageItem UpdateImageItem(IImageItem updatedItem, BitmapImage newImage);
        
        void AddImageItems(List<string> fileNames);
    }
}
