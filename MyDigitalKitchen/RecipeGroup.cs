
using MyDigitalKitchen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDigitalKitchen
{
    public class RecipeGroup: INotifyPropertyChanged
    {

        public string Letter { get; set; }
        public bool _isExpanded;
        private ObservableCollection<Recipe> _recipes;
        public bool IsExpanded 
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    // Notify that IsExpanded has changed
                    OnPropertyChanged(nameof(IsExpanded));
                    // Notify that Recipes should be re-evaluated
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
            _recipes = new ObservableCollection<Recipe>(recipes);

            ExpandCommand = new Command(Toggle);
        }

        public void Toggle() 
        {
            IsExpanded = !IsExpanded;

            OnPropertyChanged(nameof(IsExpanded));

            OnPropertyChanged(nameof(Recipes));

            Console.WriteLine($"Toggled {Letter}: IsExpanded = {IsExpanded}, Recipe Count = {Recipes.Count}");
        }

       

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            // If PropertyChanged is not null, raise the event
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
