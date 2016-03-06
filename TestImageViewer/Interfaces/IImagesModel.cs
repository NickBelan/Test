using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestImageViewer.Interfaces
{
    public interface IImagesModel
    {
        ObservableCollection<IImageItem> ImageItems { get; }
        IImageItem UpdateImageItem(IImageItem updatedImageItem, string filePath);
        void AddImageItems(List<string> fileNames);
    }
}
