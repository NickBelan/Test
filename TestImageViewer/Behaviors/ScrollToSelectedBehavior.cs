using System;
using System.Windows;
using System.Windows.Controls;

namespace TestImageViewer.Behaviors
{
    /// <summary>
    /// Allows ListView scrolling to a selected item
    /// </summary>
    public static class ScrollToSelectedBehavior
    {
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.RegisterAttached(
            "SelectedValue",
            typeof(object),
            typeof(ScrollToSelectedBehavior),
            new PropertyMetadata(null, OnSelectedValueChange));

        public static void SetSelectedValue(DependencyObject source, object value)
        {
            source.SetValue(SelectedValueProperty, value);
        }

        public static object GetSelectedValue(DependencyObject source)
        {
            return source.GetValue(SelectedValueProperty);
        }

        private static void OnSelectedValueChange(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ListView list = source as ListView;
            if (list == null)
            {
                throw new ArgumentException("dependencyObject");
            }

            list.ScrollIntoView(e.NewValue);
        }
    }
}
