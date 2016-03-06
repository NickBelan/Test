using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestImageViewer.Interfaces
{
    interface IImageItemsViewModel
    {
        ObservableCollection<IImageItem> ImageItems { get; }

        IImageItem SelectedImageItem { get; set; }

        IList<string> KnownFileTypes { get; }

        bool PreviewModeOn { get; set; }

        void AddImageItems(List<string> fileNames);

        bool PreviousImageItemAvailable { get; }

        bool NextImageItemAvailable { get; }

        void SelectNextImageItem();

        void SelectPreviousImageItem();

        void ApplyBlurFilter();
    }
}
