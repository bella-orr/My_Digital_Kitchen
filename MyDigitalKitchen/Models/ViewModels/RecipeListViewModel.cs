using MyDigitalKitchen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDigitalKitchen.Models.ViewModels
{
    public class RecipeListViewModel
    {
        public ObservableCollection<RecipeGroup> GroupedRecipes { get; set; }
        private List<Recipe> _allRecipes = new();

        private string _selectedMealType = "All";
        private string _selectedDateSort = "Newest First";

        //need to create database that stores the recipes
        public async void LoadAndGroupRecipes()
        {
            _allRecipes = await MauiProgram.RecipeDb.GetRecipesAsync();
            ApplyFilters();
        }

        public void SetMealTypeFilter(string mealType)
        {
            _selectedMealType = mealType;
            ApplyFilters();
        }

        public void SetDateSort(string sortOption)
        {
            _selectedDateSort = sortOption;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered = _allRecipes.AsEnumerable();

            // Filter by Meal Type
            if (_selectedMealType != "All")
                filtered = filtered.Where(r => r.MealType == _selectedMealType);

            // Sort by Date
            if (_selectedDateSort == "Newest First")
                filtered = filtered.OrderByDescending(r => r.DateAdded);
            else
                filtered = filtered.OrderBy(r => r.DateAdded);

            // Group by First Letter
            var grouped = filtered
                .GroupBy(r => r.Title.Substring(0, 1).ToUpper())
                .OrderBy(g => g.Key)
                .Select(g => new RecipeGroup(g.Key, g.ToList()));

            GroupedRecipes.Clear();
            foreach (var group in grouped)
            {
                GroupedRecipes.Add(group);
            }

            OnPropertyChanged(nameof(GroupedRecipes));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
