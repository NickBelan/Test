using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using Microsoft.Win32;
using TestImageViewer.Interfaces;

namespace TestImageViewer.Helpers
{
    /// <summary>
    /// Provides list of known file types and method for its verification
    /// </summary>
    public class FileTypesVerifier : IFileTypesVerifier
    {
        private const string PhotoViewerFileAssociationsPath = @"SOFTWARE\Microsoft\Windows Photo Viewer\Capabilities\FileAssociations";
        
        public IList<string> FileTypes { get; private set; }

        public FileTypesVerifier()
        {
            FileTypes = new List<string>();
            if (!ReadFileAccociationsFromRegistry())
            {
                FileTypes.Add(".jpg");
                FileTypes.Add(".jpeg");
                FileTypes.Add(".bmp");
            }
        }

        public bool IsFileTypeKnown(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if (String.IsNullOrEmpty(extension))
            {
                return false;
            }

            if (FileTypes.Any(f => f.ToUpper() == extension.ToUpper()))
            {
                return true;
            }
            return false;
        }

        private bool ReadFileAccociationsFromRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(PhotoViewerFileAssociationsPath))
                {
                    if (key != null)
                    {
                        var fileTypes = key.GetValueNames();
                        FileTypes = new List<string>(fileTypes);
                    }
                }
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }

        }
    }
}
