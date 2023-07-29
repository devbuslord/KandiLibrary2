using KandiLibrary.Models;
using KandiLibrary.MVVM;
using KandiLibrary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;

namespace KandiLibrary.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties

        // Event to handle property changed notifications
        public event PropertyChangedEventHandler PropertyChanged;

        // Collection of categories
        private ObservableCollection<string> categories;
        public ObservableCollection<string> Categories
        {
            get { return categories; }
            set { categories = value; NotifyPropertyChanged(); }
        }

        // Property to hold the new category name
        private string newCategoryName;
        public string NewCategoryName
        {
            get { return newCategoryName; }
            set { newCategoryName = value; NotifyPropertyChanged(); }
        }

        // Property to store the selected category
        private string selectedCategory;
        public string SelectedCategory
        {
            get { return selectedCategory; }
            set { selectedCategory = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<string> categoriesForComboBox;
        public ObservableCollection<string> CategoriesForComboBox
        {
            get { return categoriesForComboBox; }
            set { categoriesForComboBox = value; NotifyPropertyChanged(); }
        }

        // Command to add a new category
        public RelayCommand AddCategoryCommand { get; }

        // Command to delete a category
        public RelayCommand DeleteCategoryCommand { get; }

        // Command to add a new word
        public RelayCommand AddCommand { get; }

        // Command to delete a word
        public RelayCommand DeleteWordCommand { get; }

        private LibraryData libraryData;

        // Collection of words displayed in the DataGrid
        private ObservableCollection<Words> words;
        public ObservableCollection<Words> Words
        {
            get { return words; }
            set { words = value; NotifyPropertyChanged(); }
        }

        public class LibraryData
        {
            public List<string> Categories { get; set; }
            public List<Words> Words { get; set; }
        }

        // List of common text queries
        public ObservableCollection<string> CommonTextQueries { get; set; }

        // Properties to store the filter criteria for each ComboBox
        private string filterCriteria1;
        public string FilterCriteria1
        {
            get { return filterCriteria1; }
            set { filterCriteria1 = value; NotifyPropertyChanged(); FilterWords(); }
        }

        private string filterCriteria2;
        public string FilterCriteria2
        {
            get { return filterCriteria2; }
            set { filterCriteria2 = value; NotifyPropertyChanged(); FilterWords(); }
        }

        private string filterCriteria3;
        public string FilterCriteria3
        {
            get { return filterCriteria3; }
            set { filterCriteria3 = value; NotifyPropertyChanged(); FilterWords(); }
        }

        // Collection to store the filtered words
        private ObservableCollection<Words> filteredWords;
        public ObservableCollection<Words> FilteredWords
        {
            get { return filteredWords; }
            set { filteredWords = value; NotifyPropertyChanged(); }
        }

        // Properties to store the selected common query for each ComboBox
        private string selectedCommonQuery1;
        public string SelectedCommonQuery1
        {
            get { return selectedCommonQuery1; }
            set { selectedCommonQuery1 = value; NotifyPropertyChanged(); FilterWords(); }
        }

        private string selectedCommonQuery2;
        public string SelectedCommonQuery2
        {
            get { return selectedCommonQuery2; }
            set { selectedCommonQuery2 = value; NotifyPropertyChanged(); FilterWords(); }
        }

        private string selectedCommonQuery3;
        public string SelectedCommonQuery3
        {
            get { return selectedCommonQuery3; }
            set { selectedCommonQuery3 = value; NotifyPropertyChanged(); FilterWords(); }
        }

        #endregion

        #region Constructor

        // Constructor for the main window view model
        public MainWindowViewModel()
        {
            // Initialize collections
            Categories = new ObservableCollection<string>();
            Words = new ObservableCollection<Words>();
            FilteredWords = new ObservableCollection<Words>(Words);

            // Initialize commands
            AddCategoryCommand = new RelayCommand(AddCategory);
            DeleteWordCommand = new RelayCommand(DeleteWord);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory);
            AddCommand = new RelayCommand(AddWord, CanAddWord);

            // Create an instance of LibraryData
            libraryData = new LibraryData();

            // Initialize CategoriesForComboBox with "Select a Category" item
            CategoriesForComboBox = new ObservableCollection<string>(Categories);
            CategoriesForComboBox.Insert(0, "Select a Category");

            // Set the SelectedCategory to "Select a Category"
            SelectedCategory = "Select a Category";

            // Initialize CommonTextQueries with the common text queries
            CommonTextQueries = new ObservableCollection<string>
        {
            "Contains",
            "Does Not Contain",
            "Matches",
            "Matches Exact",
            "Does Not Match",
            // Add other common queries here
        };


            // Load data from file
            LoadDataFromFile();
        }

        #endregion

        #region Methods

        // Method to filter the words based on the selected criteria
        public void FilterWords()
        {
            // Start with all the words from the main collection (Words)
            IEnumerable<Words> filtered = Words;

            // Apply filtering based on the selected criteria in the ComboBoxes
            if (!string.IsNullOrWhiteSpace(FilterCriteria1) && !string.IsNullOrWhiteSpace(SelectedCommonQuery1))
            {
                filtered = filtered.Where(word => FilterWord(word, FilterCriteria1, SelectedCommonQuery1));
            }
            if (!string.IsNullOrWhiteSpace(FilterCriteria2) && !string.IsNullOrWhiteSpace(SelectedCommonQuery2))
            {
                filtered = filtered.Where(word => FilterWord(word, FilterCriteria2, SelectedCommonQuery2));
            }
            if (!string.IsNullOrWhiteSpace(FilterCriteria3) && !string.IsNullOrWhiteSpace(SelectedCommonQuery3))
            {
                filtered = filtered.Where(word => FilterWord(word, FilterCriteria3, SelectedCommonQuery3));
            }

            // Update the FilteredWords collection with the filtered result
            FilteredWords = new ObservableCollection<Words>(filtered);
        }


        // Helper method to apply the filtering logic for each word
        private bool FilterWord(Words word, string filterCriteria, string queryOption)
        {
            // Convert both the word and the filter criteria to lowercase (or uppercase)
            string lowerCaseWord = word.Word.ToLower();
            string lowerCaseFilterCriteria = filterCriteria.ToLower();

            // Implement your filtering logic here based on the selected filter criteria
            switch (queryOption)
            {
                case "Contains":
                    return lowerCaseWord.Contains(lowerCaseFilterCriteria);
                case "Does Not Contain":
                    return !lowerCaseWord.Contains(lowerCaseFilterCriteria);
                case "Matches":
                    return lowerCaseWord.Equals(lowerCaseFilterCriteria);
                case "Matches Exact":
                    return lowerCaseWord == lowerCaseFilterCriteria;
                case "Does Not Match":
                    return lowerCaseWord != lowerCaseFilterCriteria;
                default:
                    // If the selected filter criteria is not recognized, return true to include the word
                    return true;
            }
        }


        // Method to add a new category to the Categories collection
        private void AddCategory(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewCategoryName) && !Categories.Contains(NewCategoryName))
            {
                Categories.Add(NewCategoryName);
                SaveDataToFile();
                NewCategoryName = string.Empty; // Clear the input after adding the category

                // Update CategoriesForComboBox collection
                CategoriesForComboBox = new ObservableCollection<string>(Categories);
                CategoriesForComboBox.Insert(0, "Select a Category");
                UpdateCategoriesForComboBox();
            }
        }

        private void DeleteCategory(object parameter)
        {
            if (parameter is string selectedCategory)
            {
                Categories.Remove(selectedCategory);
                UpdateCategoriesForComboBox();
                SaveDataToFile();
            }
        }

        // Method to delete a category from the Categories collection
        private void DeleteWord(object parameter)
        {
            if (parameter is Words selectedWord)
            {
                Words.Remove(selectedWord);
                SaveDataToFile();
            }
        }

        // Method to add a new word to the Words collection
        private void AddWord(object parameter)
        {
            string newWordText = parameter as string;
            if (!string.IsNullOrWhiteSpace(newWordText))
            {
                var newWord = new Words
                {
                    Word = newWordText,
                    Category = SelectedCategory != "Select a Category" ? SelectedCategory : null
                };
                Words.Add(newWord);
                SaveDataToFile();
                //SelectedCategory = "Select a Category"; // Reset the selected category to the default option
            }
        }

        // Method to check if a word can be added
        private bool CanAddWord(object parameter)
        {
            return !string.IsNullOrWhiteSpace(parameter as string);
        }

        private void UpdateCategoriesForComboBox()
        {
            CategoriesForComboBox = new ObservableCollection<string>(Categories);
            CategoriesForComboBox.Insert(0, "Select a Category");
        }

        // Helper method to raise the PropertyChanged event when a property changes
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void SaveDataToFile()
        {
            try
            {
                // Create a data object to hold the Categories and Words
                var data = new
                {
                    Categories = Categories.ToList(),
                    Words = Words.ToList()
                };

                // Convert the data object to JSON
                string jsonData = JsonSerializer.Serialize(data);

                // Get the path to the user's AppData/Local folder
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                // Create the full path to the file
                string filePath = Path.Combine(appDataFolder, "KandiLibraryData.json");

                // Write the JSON data to the file
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the save process
                // For example, you can display an error message to the user
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDataFromFile()
        {
            try
            {
                // Get the path to the user's AppData/Local folder
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                // Create the full path to the file
                string filePath = Path.Combine(appDataFolder, "KandiLibraryData.json");

                // Read the JSON data from the file
                string jsonData = File.ReadAllText(filePath);

                // Deserialize JSON data and populate the libraryData object
                libraryData = JsonSerializer.Deserialize<LibraryData>(jsonData);

                // Clear existing data and populate the Categories and Words collections from the libraryData object
                Categories.Clear();
                Words.Clear();
                foreach (string category in libraryData.Categories)
                {
                    Categories.Add(category);
                }
                foreach (Words word in libraryData.Words)
                {
                    Words.Add(word);
                }
                UpdateCategoriesForComboBox();
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the load process
                // For example, you can display an error message to the user
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
