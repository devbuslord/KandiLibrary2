using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KandiLibrary.Models
{
    internal class Words : INotifyPropertyChanged
    {
        private string word;
        public string Word
        {
            get { return word; }
            set { word = value; NotifyPropertyChanged(); }
        }

        private string category;
        public string Category
        {
            get { return category; }
            set { category = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
