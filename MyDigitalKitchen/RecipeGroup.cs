
using MyDigitalKitchen.Helpers;
using MyDigitalKitchen.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MyDigitalKitchen
{
    public class RecipeGroup : INotifyPropertyChanged
    {
        public string Letter { get; set; }
        private bool _isExpanded;
        private ObservableCollection<Recipe> _recipes;

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
                    OnPropertyChanged(nameof(Recipes));
                }
            }
        }

        public ObservableCollection<Recipe> Recipes => IsExpanded ? _recipes : new ObservableCollection<Recipe>();

        public ICommand ExpandCommand { get; }

        public RecipeGroup(string letter, List<Recipe> recipes)
        {
            Letter = letter;
            IsExpanded = false;
            _recipes = new ObservableCollection<Recipe>(recipes.ToObservableCollection()); 
            ExpandCommand = new Command(Toggle);
        }

        public void Toggle()
        {
            IsExpanded = !IsExpanded;
            OnPropertyChanged(nameof(IsExpanded));
            OnPropertyChanged(nameof(Recipes));
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}