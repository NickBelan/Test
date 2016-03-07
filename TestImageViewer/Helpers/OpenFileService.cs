using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using TestImageViewer.Interfaces;

namespace TestImageViewer.Helpers
{
    /// <summary>
    /// Provides possibility to browse files, filtered by type
    /// </summary>
    public class OpenFileService : IOpenFileService
    {
        public IList<string> OpenFileDialog(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = filter
            };
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileNames.ToList();
            }
            return new List<string>();
        }
    }
}
