using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestImageViewer.Helpers;
using TestImageViewer.Interfaces;

namespace TestImageViewer.ViewModels
{
    public class ImageItemsViewModel : Notifier, IImageItemsViewModel
    {
        public const string SelectedItemPropertyName = "SelectedImageItem";
        public const string NextImageItemAvailablePropertyName = "NextImageItemAvailable";
        public const string PreviousImageItemAvailablePropertyName = "PreviousImageItemAvailable";
        private const string PreviewModeOnPropertyName = "PreviewModeOn";

        private readonly IImagesModel model;
        private IImageItem selectedImageItem;
        private bool nextImageItemAvailable;
        private bool previousImageItemAvailable;
        private bool isPreviewModeOn;

        private readonly FileTypesVerifier fileTypesVerifier;

        public ImageItemsViewModel(IImagesModel imageItemModel)
        {
            model = imageItemModel;
            fileTypesVerifier = new FileTypesVerifier();
        }

        #region IImageItemsViewModel

        public ObservableCollection<IImageItem> ImageItems
        {
            get { return model.ImageItems; }
        }

        public IImageItem SelectedImageItem
        {
            get { return selectedImageItem; }
            set
            {
                selectedImageItem = value;
                NotifyPropertyChanged(SelectedItemPropertyName);
                CheckNavigationAvailability();
            }
        }

        public bool NextImageItemAvailable
        {
            get { return nextImageItemAvailable; }
            set
            {
                nextImageItemAvailable = value;
                NotifyPropertyChanged(NextImageItemAvailablePropertyName);
            }
        }

        public bool PreviousImageItemAvailable
        {
            get { return previousImageItemAvailable; }
            set
            {
                previousImageItemAvailable = value;
                NotifyPropertyChanged(PreviousImageItemAvailablePropertyName);
            }
        }

        public IList<string> KnownFileTypes
        {
            get { return fileTypesVerifier.FileTypes; }
        }

        public bool PreviewModeOn
        {
            get { return isPreviewModeOn; }
            set
            {
                isPreviewModeOn = value;
                NotifyPropertyChanged(PreviewModeOnPropertyName);
            }
        }

        public void AddImageItems(List<string> fileNames)
        {
            model.AddImageItems(fileNames.Where(f => fileTypesVerifier.IsFileTypeKnown(f)).ToList());
            if (model.ImageItems.Count > 0)
                SelectedImageItem = model.ImageItems.Last();
        }

        public void SelectNextImageItem()
        {
            int currentIndex;
            if (SelectedImageItem == null || (currentIndex = ImageItems.IndexOf(SelectedImageItem)) == -1)
            {
                return;
            }

            currentIndex++;
            if (ImageItems.Count > currentIndex)
            {
                SelectedImageItem = ImageItems[currentIndex];
            }
        }

        public void SelectPreviousImageItem()
        {
            int currentIndex;
            if (SelectedImageItem == null || (currentIndex = ImageItems.IndexOf(SelectedImageItem)) == -1)
            {
                return;
            }

            currentIndex--;
            if (currentIndex >= 0)
            {
                SelectedImageItem = ImageItems[currentIndex];
            }
        }

        public void ApplyBlurFilter()
        {
            if (SelectedImageItem == null)
            {
                return;
            }

            string newfilePath = BlurFilterHelper.ApplyBlur(SelectedImageItem.Image, SelectedImageItem.FilePath);
            if (!String.IsNullOrEmpty(newfilePath))
            {
                SelectedImageItem = model.UpdateImageItem(SelectedImageItem, newfilePath);
            }
        }

        #endregion IImageItemsViewModel

        private void CheckNavigationAvailability()
        {
            int currentIndex;
            if (SelectedImageItem == null || (currentIndex = ImageItems.IndexOf(SelectedImageItem)) == -1)
            {
                NextImageItemAvailable = false;
                PreviousImageItemAvailable = false;
                return;
            }

            NextImageItemAvailable = ImageItems.Count > (currentIndex + 1);
            PreviousImageItemAvailable = (currentIndex - 1) >= 0;
        }

    }
}
