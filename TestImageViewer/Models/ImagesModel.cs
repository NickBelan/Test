using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TestImageViewer.Interfaces;

namespace TestImageViewer.Models
{
    public class ImagesModel : IImagesModel
    {
        public ObservableCollection<IImageItem> ImageItems { get; set; }

        public ImagesModel()
        {
            ImageItems = new ObservableCollection<IImageItem>();
        }

        public void AddImageItems(List<string> fileNames)
        {
            if (fileNames == null)
                throw new ArgumentException("filePath");

            foreach (string filePath in fileNames)
            {
                string path = filePath;
                Task<ImageItem>.Factory.StartNew(() => new ImageItem(path)).ContinueWith(
                    task => ImageItems.Add(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public IImageItem UpdateImageItem(IImageItem updatedItem, BitmapImage newImage)
        {
            return GetImageItem(updatedItem.Id).Update(newImage);
        }

        private IImageItem GetImageItem(Guid projectId)
        {
            return ImageItems.FirstOrDefault(
                project => project.Id == projectId);
        }
    }
}
