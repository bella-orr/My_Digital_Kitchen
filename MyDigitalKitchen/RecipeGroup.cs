using MyDigitalKitchen.Helpers;
using MyDigitalKitchen.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MyDigitalKitchen
{
    // Represents a group of recipes (e.g., recipes starting with 'A').
    // Used for grouping in the RecipeList UI.
    public class RecipeGroup : INotifyPropertyChanged
    {
        // The letter/key for this group.
        public string Letter { get; set; }

        // Tracks if the group is expanded in the UI.
        private bool _isExpanded;

        // Collection of recipes in this group.
        private ObservableCollection<Recipe> _recipes;

        // Controls group expansion, notifies UI.
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
                    OnPropertyChanged(nameof(Recipes)); // Recipes property depends on IsExpanded
                }
            }
        }

        // Returns recipes only if expanded.
        public ObservableCollection<Recipe> Recipes => IsExpanded ? _recipes : new ObservableCollection<Recipe>();

        // Command to toggle expansion via UI interaction.
        public ICommand ExpandCommand { get; }

        // Constructor: sets up group with letter and recipes.
        public RecipeGroup(string letter, List<Recipe> recipes)
        {
            Letter = letter;
            IsExpanded = false; // Default state is collapsed.
            _recipes = new ObservableCollection<Recipe>(recipes.ToObservableCollection()); // Convert list to observable collection.
            ExpandCommand = new Command(Toggle); // Command linked to Toggle method.
        }

        // Toggles the IsExpanded state.
        public void Toggle()
        {
            IsExpanded = !IsExpanded;
            // PropertyChanged is handled in the setter.
        }

        // Event needed for INotifyPropertyChanged.
        public event PropertyChangedEventHandler PropertyChanged;

        // Helper to raise PropertyChanged event.
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}