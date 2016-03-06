using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using TestImageViewer.Interfaces;
using TestImageViewer.Models;
using TestImageViewer.ViewModels;

namespace TestImageViewer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IImageItemsViewModel imageItemsViewModel;

        public MainWindow()
        {
            InitializeComponent();

            InitViewModel();
            ApplyBindings();
            ApplyCommandBindings();
        }

        #region Initializers

        private void InitViewModel()
        {
            IImagesModel imageItemsModel = new ImagesModel();
            imageItemsViewModel = new ImageItemsViewModel(imageItemsModel);
            ImagesListView.ItemsSource = imageItemsViewModel.ImageItems;
            DataContext = imageItemsViewModel;
        }

        private void ApplyBindings()
        {
            Binding myBinding = new Binding
            {
                Source = imageItemsViewModel,
                Path = new PropertyPath(ImageItemsViewModel.SelectedItemPropertyName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(ImagesListView, Selector.SelectedItemProperty, myBinding);

            Binding imageBinding = new Binding
            {
                Source = imageItemsViewModel,
                Path = new PropertyPath(ImageItemsViewModel.SelectedItemPropertyName),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Converter = new Converters.ImageSourceConverter()
            };
            BindingOperations.SetBinding(PreviewImage, Image.SourceProperty, imageBinding);

            Binding buttonUpBinding = new Binding
            {
                Source = imageItemsViewModel,
                Path = new PropertyPath(ImageItemsViewModel.PreviousImageItemAvailablePropertyName),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(ScrollUpButton, IsEnabledProperty, buttonUpBinding);


            Binding buttonDownBinding = new Binding
            {
                Source = imageItemsViewModel,
                Path = new PropertyPath(ImageItemsViewModel.NextImageItemAvailablePropertyName),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(ScrollDownButton, IsEnabledProperty, buttonDownBinding);            
        }

        private void ApplyCommandBindings()
        {
            CommandBinding bind = new CommandBinding(ApplicationCommands.Open);
            bind.Executed += OnOpenCommandExecuted;
            CommandBindings.Add(bind);    
        }

        #endregion Initializers

        #region Drag and Drop
        private void Thumbnails_OnPreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
        }

        private void OnThumbnailsDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
                imageItemsViewModel.AddImageItems(files.ToList());
        }

        #endregion Drag and Drop

        #region EventHandler methods

        void OnOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = String.Concat("All types (*", String.Join(",*", imageItemsViewModel.KnownFileTypes), ")|*", String.Join(";*", imageItemsViewModel.KnownFileTypes))
            };
            if (openFileDialog.ShowDialog() == true)
            {
                imageItemsViewModel.AddImageItems(openFileDialog.FileNames.ToList());
            }
        }

        private void OnImagesListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImagesListView.ScrollIntoView(ImagesListView.SelectedItem);
        }

        private void OnImagesListViewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SwitchToPreviewMode();
        }

        private void OnScrollUpButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollUp();
        }

        private void OnScrollDownButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollDown();
        }

        private void OnBlurButtonClick(object sender, RoutedEventArgs e)
        {
            imageItemsViewModel.ApplyBlurFilter();
        }

        #endregion  EventHandler methods

        #region Methods

        private void ScrollUp()
        {
            imageItemsViewModel.SelectPreviousImageItem();
        }

        private void ScrollDown()
        {
            imageItemsViewModel.SelectNextImageItem();
        }

        private void SwitchToPreviewMode()
        {
            if (imageItemsViewModel.SelectedImageItem != null)
            {
                imageItemsViewModel.PreviewModeOn = true;
            }

        }

        private void OnMainWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (imageItemsViewModel.PreviewModeOn)
            {
                if (e.Key == Key.Escape)
                {
                    imageItemsViewModel.PreviewModeOn = false;
                    ImagesListView.Focus();
                }
                if (e.Key == Key.Up && ScrollUpButton.IsEnabled)
                {
                    ScrollUp();
                    e.Handled = true;
                }
                if (e.Key == Key.Down && ScrollDownButton.IsEnabled)
                {
                    ScrollDown();
                    e.Handled = true;
                }
            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    SwitchToPreviewMode();
                }
            }
        }

        #endregion Methods
    }
}
