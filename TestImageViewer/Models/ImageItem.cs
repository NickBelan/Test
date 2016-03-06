using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using TestImageViewer.Helpers;
using TestImageViewer.Interfaces;

namespace TestImageViewer.Models
{
    public class ImageItem : Notifier, IImageItem
    {
        private const string ImagePropertyName = "Image";
        private BitmapImage image;
        
        public string FilePath { get; private set; }

        public BitmapImage Image
        {
            get { return image; }
            private set
            {
                image = value;
                NotifyPropertyChanged(ImagePropertyName);
            }
        }

        public Guid Id { get; private set; }

        public ImageItem(string filePath)
        {
            Id = Guid.NewGuid();
            LoadImage(filePath);
        }

        private void LoadImage(string filePath)
        {
            Image = new BitmapImage(new Uri(filePath));
            FilePath = filePath;
        }

        public IImageItem Update(IImageItem updatedItem, string newFilePath)
        {
            LoadImage(newFilePath);
            return this;
        }
    }
}
