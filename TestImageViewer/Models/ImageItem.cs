using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using TestImageViewer.Helpers;
using TestImageViewer.Interfaces;

namespace TestImageViewer.Models
{
    public class ImageItem : Notifier, IImageItem
    {
        private const string ImagePropertyName = "Image";
        private const string IsBlurredPropertyName = "IsBlurred";
        private const int ThumbnailSize = 200;

        private bool isBlurred;
        private BitmapImage image;
        private BitmapImage fullImage;
        private BitmapImage blurredImage;
        private BitmapImage blurredThumbnailImage;

        public Guid Id { get; private set; }

        public string FilePath { get; private set; }

        public bool IsBlurred 
        {
            get { return isBlurred; }
            set
            {
                isBlurred = value;
                NotifyPropertyChanged(IsBlurredPropertyName);
                NotifyPropertyChanged(ImagePropertyName);
            }
        }

        public bool HasBlurredImage
        {
            get { return (blurredImage != null); }
        }

        public BitmapImage Image
        {
            get
            {
                if (IsBlurred)
                {
                    return blurredThumbnailImage;
                }
                return image;
            }
            private set
            {
                image = value;
                NotifyPropertyChanged(ImagePropertyName);
            }
        }

        public BitmapImage FullImage
        {
            get
            {
                if (IsBlurred)
                {
                    return blurredImage;
                }
                if (fullImage != null)
                {
                    return fullImage;
                }
                return fullImage = LoadFullImage(FilePath);
            }
        }

        public void ClearFullImage()
        {
            fullImage = null;
        }

        #region Methods

        public ImageItem(string filePath)
        {
            Id = Guid.NewGuid();
            LoadImage(filePath);
        }

        private void LoadImage(string filePath)
        {
            var thumbnail = CreateThumbnail(filePath);
            thumbnail.Freeze();
            Image = thumbnail;

            FilePath = filePath;
        }

        private BitmapImage LoadFullImage(string filePath)
        {
            return BitmapHelper.ConvertBitmap2BitmapImage(new Bitmap(LoadImageFromFile(filePath)));
        }

        public IImageItem Update(BitmapImage newBlurredImage)
        {
            blurredImage = newBlurredImage;
            blurredThumbnailImage = CreateThumbnail(newBlurredImage);
            return this;
        }

        private BitmapImage CreateThumbnail(BitmapImage bitmapImage)
        {
            return CreateThumbnailImage(BitmapHelper.ConvertBitmapImage2Bitmap(bitmapImage));
        }

        private BitmapImage CreateThumbnail(string fileName)
        {
            return CreateThumbnailImage(LoadImageFromFile(fileName));
        }

        private BitmapImage CreateThumbnailImage(Image imageToConvert)
        {
            Image thumbnail = BitmapHelper.GetThumbnailImage(imageToConvert, ThumbnailSize, ThumbnailSize);
            return BitmapHelper.ConvertBitmap2BitmapImage(new Bitmap(thumbnail));
        }

        private Image LoadImageFromFile(string fileName)
        {
            return BitmapHelper.ReadImageWithOrientation(fileName);
        }

        #endregion Methods
    }
}
