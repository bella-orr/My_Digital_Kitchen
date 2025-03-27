using System.Collections.ObjectModel;
using MyDigitalKitchen.Helpers;

namespace MyDigitalKitchen
{
    public class RecipeRepository
    {
        private ObservableCollection<Recipe> _recipes = new ObservableCollection<Recipe>();
        private int _nextId = 1;

        // Get All Recipes
        public ObservableCollection<Recipe> GetAllRecipes()
        {
            return _recipes;
        }

        // Get Recipe by ID
        public Recipe GetRecipeById(int id)
        {
            return _recipes.FirstOrDefault(r => r.Id == id);
        }

        // Add a Recipe
        public void AddRecipe(Recipe recipe)
        {
            recipe.Id = _nextId++;
            _recipes.Add(recipe);
        }

        // Update a Recipe
        public void UpdateRecipe(Recipe recipe)
        {
            var existingRecipe = _recipes.FirstOrDefault(r => r.Id == recipe.Id);
            if (existingRecipe != null)
            {
                int index = _recipes.IndexOf(existingRecipe);
                _recipes[index] = recipe;
            }
        }

        // Delete a Recipe
        public void DeleteRecipe(int id)
        {
            var recipeToRemove = _recipes.FirstOrDefault(r => r.Id == id);
            if (recipeToRemove != null)
            {
                _recipes.Remove(recipeToRemove);
            }
        }

        // Search Recipes by Keyword
        public ObservableCollection<Recipe> SearchRecipes(string keyword)
        {
            return new ObservableCollection<Recipe>(_recipes.Where(r =>
                r.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                r.Ingredients.Any(i => i.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ));
        }

        // Get Favorite Recipes
        public ObservableCollection<Recipe> GetFavorites()
        {
            return new ObservableCollection<Recipe>(_recipes.Where(r => r.IsFavorite));
        }

        // Get Recently Accessed Recipes (Top 5)
        public ObservableCollection<Recipe> GetRecentlyAccessed()
        {
            return new ObservableCollection<Recipe>(_recipes.OrderByDescending(r => r.LastAccessed).Take(5).ToObservableCollection());
        }

        // Get Recipes by Meal Type
        public ObservableCollection<Recipe> GetRecipesByMealType(string mealType)
        {
            return new ObservableCollection<Recipe>(_recipes.Where(r => r.MealType == mealType));
        }
    }
}
