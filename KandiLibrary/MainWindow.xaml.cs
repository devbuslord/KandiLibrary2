using KandiLibrary.ViewModels;
using KandiLibrary.Views;
using System.Windows;

namespace KandiLibrary
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            // Create an instance of the MainWindowViewModel and set it as the DataContext
            MainWindowViewModel viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            SearchView.DataContext = viewModel;
        }

        private void btnCategories_Click(object sender, RoutedEventArgs e)
        {
            CategoryView.Visibility = Visibility.Visible;
            SearchView.Visibility = Visibility.Collapsed;
            LibraryView.Visibility = Visibility.Collapsed;
        }

        private void btnLibrary_Click(object sender, RoutedEventArgs e)
        {
            LibraryView.Visibility = Visibility.Visible;
            SearchView.Visibility = Visibility.Collapsed;
            CategoryView.Visibility = Visibility.Collapsed;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchView.Visibility = Visibility.Visible;
            LibraryView.Visibility = Visibility.Collapsed;
            CategoryView.Visibility = Visibility.Collapsed;
        }
    }
}