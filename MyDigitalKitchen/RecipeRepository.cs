using System.Collections.ObjectModel;
using MyDigitalKitchen.Helpers;
using MyDigitalKitchen.Models;

namespace MyDigitalKitchen
{
    public class RecipeRepository
    {
        private ObservableCollection<Recipe> _recipes = new ObservableCollection<Recipe>();
        private int _nextId = 1;

      
        public ObservableCollection<Recipe> GetAllRecipes()
        {
            return _recipes;
        }

     
        public Recipe GetRecipeById(int id)
        {
            return _recipes.FirstOrDefault(r => r.Id == id);
        }

       
        public void AddRecipe(Recipe recipe)
        {
            recipe.Id = _nextId++;
            _recipes.Add(recipe);
        }

      
        public void UpdateRecipe(Recipe recipe)
        {
            var existingRecipe = _recipes.FirstOrDefault(r => r.Id == recipe.Id);
            if (existingRecipe != null)
            {
                int index = _recipes.IndexOf(existingRecipe);
                _recipes[index] = recipe;
            }
        }

      
        public void DeleteRecipe(int id)
        {
            var recipeToRemove = _recipes.FirstOrDefault(r => r.Id == id);
            if (recipeToRemove != null)
            {
                _recipes.Remove(recipeToRemove);
            }
        }

       
        public ObservableCollection<Recipe> SearchRecipes(string keyword)
        {
            return new ObservableCollection<Recipe>(_recipes.Where(r =>
                r.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                r.Ingredients.Any(i => i.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ));
        }

       
        public ObservableCollection<Recipe> GetFavorites()
        {
            return new ObservableCollection<Recipe>(_recipes.Where(r => r.IsFavorite));
        }

        
        public ObservableCollection<Recipe> GetRecentlyAccessed()
        {
            return new ObservableCollection<Recipe>(_recipes.OrderByDescending(r => r.LastAccessed).Take(5).ToObservableCollection());
        }

        
        public ObservableCollection<Recipe> GetRecipesByMealType(string mealType)
        {
            return new ObservableCollection<Recipe>(_recipes.Where(r => r.MealType == mealType));
        }
    }
}
