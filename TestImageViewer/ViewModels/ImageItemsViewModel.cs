using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TestImageViewer.Helpers;
using TestImageViewer.Interfaces;
using TestImageViewer.Models;

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

        private DelegateCommand<string> scrollUpCommand;
        private DelegateCommand<string> scrollDownCommand;
        private DelegateCommand<string> applyBlurCommand;
        private DelegateCommand<string> switchToPreviewModeCommand;
        private DelegateCommand<string> switchToListViewModeCommand;
        private DelegateCommand<string> openFilesCommand;
        private DelegateCommand<IDataObject> dropFilesCommand;

        private readonly IFileTypesVerifier fileTypesVerifier;
        private readonly IOpenFileService openFileService;

        public ImageItemsViewModel(IOpenFileService openFileService, IFileTypesVerifier fileTypesVerifier)
        {
            model = new ImagesModel();
            this.fileTypesVerifier = fileTypesVerifier;
            this.openFileService = openFileService;

            InitCommands();
        }

        private void InitCommands()
        {
            scrollUpCommand = new DelegateCommand<string>(s => SelectPreviousImageItem());
            scrollDownCommand = new DelegateCommand<string>(s => SelectNextImageItem());
            applyBlurCommand = new DelegateCommand<string>(s => ApplyBlurFilter());
            switchToPreviewModeCommand = new DelegateCommand<string>(s => SwitchToPreviewMode());
            switchToListViewModeCommand = new DelegateCommand<string>(s => { PreviewModeOn = false; });
            openFilesCommand = new DelegateCommand<string>(s => OpenFiles());
            dropFilesCommand = new DelegateCommand<IDataObject>(DropFiles);
        }

        #region IImageItemsViewModel

        #region Commands

        public DelegateCommand<string> ScrollUpCommand
        {
            get { return scrollUpCommand; }
        }

        public DelegateCommand<string> ScrollDownCommand
        {
            get { return scrollDownCommand; }
        }

        public DelegateCommand<string> ApplyBlurCommand
        {
            get { return applyBlurCommand; }
        }

        public DelegateCommand<string> SwitchToPreviewModeCommand
        {
            get { return switchToPreviewModeCommand; }
        }

        public DelegateCommand<string> SwitchToListViewModeCommand
        {
            get { return switchToListViewModeCommand; }
        }

        public DelegateCommand<string> OpenFilesCommand
        {
            get { return openFilesCommand; }
        }

        public DelegateCommand<IDataObject> DropFilesCommand
        {
            get { return dropFilesCommand; }
        }

        #endregion Commands

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

        public bool PreviewModeOn
        {
            get { return isPreviewModeOn; }
            set
            {
                isPreviewModeOn = value;
                NotifyPropertyChanged(PreviewModeOnPropertyName);
            }
        }

        #endregion IImageItemsViewModel

        #region Methods

        private void AddImageItems(IEnumerable<string> fileNames)
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

        private void ApplyBlurFilter()
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

        private void SwitchToPreviewMode()
        {
            if (SelectedImageItem != null)
            {
                PreviewModeOn = true;
            }
        }

        private void OpenFiles()
        {
            string filter = String.Concat("All types (*", String.Join(",*", fileTypesVerifier.FileTypes), ")|*",
                String.Join(";*", fileTypesVerifier.FileTypes));
            IList<string> filesList = openFileService.OpenFileDialog(filter);
            if (filesList.Any())
            {
                AddImageItems(filesList);
            }
        }

        private void DropFiles(IDataObject data)
        {
            object dataValue = data.GetData(DataFormats.FileDrop);
            string[] files = dataValue as string[];
            if (files == null)
            {
                throw new ArgumentException("data");
            }
            
            if (files.Length > 0)
            {
                AddImageItems(files.ToList());
            }
        }

        #endregion Methods
    }
}
