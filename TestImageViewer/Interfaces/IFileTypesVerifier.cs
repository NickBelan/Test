using System.Collections.Generic;

namespace TestImageViewer.Interfaces
{
    public interface IFileTypesVerifier
    {
        IList<string> FileTypes { get; }

        bool IsFileTypeKnown(string filePath);
    }
}
