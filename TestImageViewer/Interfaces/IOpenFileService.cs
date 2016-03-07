using System.Collections.Generic;

namespace TestImageViewer.Interfaces
{
    public interface IOpenFileService
    {
        IList<string> OpenFileDialog(string filter);
    }
}
