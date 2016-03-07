using TestImageViewer.Helpers;
using TestImageViewer.Interfaces;
using TestImageViewer.ViewModels;

namespace TestImageViewer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            IOpenFileService openFileService = new OpenFileService();
            IFileTypesVerifier fileTypesVerifier = new FileTypesVerifier();
            DataContext = new ImageItemsViewModel(openFileService, fileTypesVerifier);
        }
       
     }
}
