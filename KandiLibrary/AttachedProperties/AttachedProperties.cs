using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace KandiLibrary.AttachedProperties
{
    public static class ListBoxExtensions
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(ListBoxExtensions),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox)
            {
                listBox.SelectionChanged -= ListBox_SelectionChanged;
                listBox.SelectionChanged += ListBox_SelectionChanged;
            }
        }

        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                SetSelectedItems(listBox, listBox.SelectedItems);
            }
        }
    }
}
