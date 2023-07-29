using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using KandiLibrary;
using KandiLibrary.ViewModels;

namespace KandiLibrary.Views
{
    public partial class ucSearch : UserControl
    {
        public ucSearch() 
        {
            InitializeComponent();
            Loaded += UcSearch_Loaded;
        }

        private void UcSearch_Loaded(object sender, RoutedEventArgs e)
        {
            // Access the DataContext (MainWindowViewModel) and call the FilterWords method to initialize the query list
            if (DataContext is ViewModels.MainWindowViewModel viewModel)
            {
                viewModel.FilterWords();
            }
        }

        // Event handler for DataGrid.Sorting event
        private void WordGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            // Cancel the default sorting behavior
            e.Handled = true;

            // Get the column that is being sorted
            DataGridColumn column = e.Column;

            // Sort the data based on the selected column
            if (column.SortDirection == ListSortDirection.Ascending)
            {
                column.SortDirection = ListSortDirection.Descending;
                SortDataGrid(column, ListSortDirection.Descending);
            }
            else
            {
                column.SortDirection = ListSortDirection.Ascending;
                SortDataGrid(column, ListSortDirection.Ascending);
            }
        }

        // Method to sort the DataGrid based on the selected column and sort direction
        private void SortDataGrid(DataGridColumn column, ListSortDirection direction)
        {
            // Get the ICollectionView of the DataGrid's ItemsSource
            ICollectionView view = CollectionViewSource.GetDefaultView(WordGrid.ItemsSource);

            // Apply sorting to the ICollectionView
            if (view != null)
            {
                view.SortDescriptions.Clear();
                string propertyName = column.SortMemberPath;
                if (!string.IsNullOrEmpty(propertyName))
                {
                    // If the property to sort is a complex property (e.g., Category.Name), 
                    // use the SortMemberPath to sort by that property
                    view.SortDescriptions.Add(new SortDescription(propertyName, direction));
                }
            }
        }

        // Event handler for txtQuery1.TextChanged
        private void TxtQuery1_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Notify the ViewModel that the FilterCriteria1 has changed
            if (this.DataContext is MainWindowViewModel viewModel)
            {
                viewModel.FilterCriteria1 = txtQuery1.Text;
            }
        }

        private void TxtQuery2_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Notify the ViewModel that the FilterCriteria2 has changed
            if (this.DataContext is MainWindowViewModel viewModel)
            {
                viewModel.FilterCriteria2 = txtQuery2.Text;
            }
        }

        private void TxtQuery3_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Notify the ViewModel that the FilterCriteria3 has changed
            if (this.DataContext is MainWindowViewModel viewModel)
            {
                viewModel.FilterCriteria3 = txtQuery3.Text;
            }
        }

    }
}
