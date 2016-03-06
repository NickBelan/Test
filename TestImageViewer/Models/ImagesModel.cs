using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                ImageItems.Add(new ImageItem(filePath));
            }
        }

        public IImageItem UpdateImageItem(IImageItem updatedItem, string newFilePath)
        {
            return GetImageItem(updatedItem.Id).Update(updatedItem, newFilePath);
        }

        private IImageItem GetImageItem(Guid projectId)
        {
            return ImageItems.FirstOrDefault(
                project => project.Id == projectId);
        }
    }
}
