using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestImageViewer.Behaviors
{
    /// <summary>
    /// Allows ListView to accept dropped files
    /// </summary>
    public static class DragAndDropFilesBehavior
    {
        public static readonly DependencyProperty DragAndDropFilesProperty = DependencyProperty.RegisterAttached(
            "DragAndDropFiles",
            typeof (ICommand),
            typeof(DragAndDropFilesBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnDragAndDropBehaviourChanged));
        public static ICommand GetDragAndDropFiles(DependencyObject source)
        {
            return (ICommand)source.GetValue(DragAndDropFilesProperty);
        }
        public static void SetDragAndDropFiles(DependencyObject source, ICommand value)
        {
            source.SetValue(DragAndDropFilesProperty, value);
        }
        private static void OnDragAndDropBehaviourChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ListView list = source as ListView;
            if (list == null)
            {
                throw new ArgumentException("dependencyObject");
            }

            list.Drop += (s, a) =>
            {
                ICommand iCommand = GetDragAndDropFiles(source);
                if (iCommand != null)
                {
                    iCommand.Execute(a.Data);
                }
            };

            list.PreviewDrop += (s, a) =>
            {
                if (a.Data.GetDataPresent(DataFormats.FileDrop))
                    a.Effects = DragDropEffects.Copy;
            };
        }
    }
}
