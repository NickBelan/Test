using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using TestImageViewer.Helpers;

namespace TestImageViewer.Interfaces
{
    interface IImageItemsViewModel
    {
        ObservableCollection<IImageItem> ImageItems { get; }

        IImageItem SelectedImageItem { get; set; }

        BitmapSource SelectedFullImage { get; }

        bool PreviewModeOn { get; set; }

        bool PreviousImageItemAvailable { get; }

        bool NextImageItemAvailable { get; }

        bool IsSelectedImageBlurred { get; set; }

        DelegateCommand<string> ScrollUpCommand { get; }

        DelegateCommand<string> ScrollDownCommand { get; }

        DelegateCommand<string> SwitchToPreviewModeCommand { get; }

        DelegateCommand<string> SwitchToListViewModeCommand { get; }
 
        DelegateCommand<string> OpenFilesCommand { get; }

        DelegateCommand<IDataObject> DropFilesCommand { get; }

    }
}
