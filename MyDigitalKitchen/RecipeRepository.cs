using System.Collections.ObjectModel;
using MyDigitalKitchen.Models;
using MyDigitalKitchen.Services;
using SQLite;

using System.Linq;
using System.Text.Json; 

namespace MyDigitalKitchen
{
    public class RecipeRepository
    {
        private readonly SQLiteAsyncConnection _dbConnection;

        public RecipeRepository(DatabaseService databaseService)
        {
            _dbConnection = databaseService.GetConnection();
        }


        private string SerializeIngredients(List<string> ingredients)
        {
            if (ingredients == null || !ingredients.Any())
                return null;
            
            return string.Join(",", ingredients.Select(item => item.Replace(",", "&#44;")));
        }

        
        private List<string> DeserializeIngredients(string ingredientsBlob)
        {
            if (string.IsNullOrEmpty(ingredientsBlob))
                return new List<string>();
            return ingredientsBlob.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(item => item.Replace("&#44;", ",").Trim())
                                  .ToList();
        }

        

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            var recipes = await _dbConnection.Table<Recipe>().ToListAsync();
            
            foreach (var recipe in recipes)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipes;
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            var recipe = await _dbConnection.Table<Recipe>().Where(r => r.Id == id).FirstOrDefaultAsync();
            
            if (recipe != null)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipe;
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            
            recipe.IngredientsBlob = SerializeIngredients(recipe.Ingredients);
            recipe.LastAccessed = DateTime.Now;
            await _dbConnection.InsertAsync(recipe);
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            
            recipe.IngredientsBlob = SerializeIngredients(recipe.Ingredients);
            await _dbConnection.UpdateAsync(recipe);
        }

        public async Task DeleteRecipeAsync(int id)
        {
            await _dbConnection.DeleteAsync<Recipe>(id);
        }

        public async Task<List<Recipe>> SearchRecipesAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetAllRecipesAsync();
            }

            
            var results = await _dbConnection.Table<Recipe>()
                .Where(r => (!string.IsNullOrEmpty(r.Title) && r.Title.ToLower().Contains(keyword.ToLower())) ||
                            (!string.IsNullOrEmpty(r.IngredientsBlob) && r.IngredientsBlob.ToLower().Contains(keyword.ToLower()))) 
                .ToListAsync();

            
            foreach (var recipe in results)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }

            return results;
        }

        public async Task<List<Recipe>> GetFavoritesAsync()
        {
            var recipes = await _dbConnection.Table<Recipe>().Where(r => r.IsFavorite).ToListAsync();
            
            foreach (var recipe in recipes)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipes;
        }

        public async Task<List<Recipe>> GetRecentlyAccessedAsync()
        {
            var recipes = await _dbConnection.Table<Recipe>()
                .OrderByDescending(r => r.LastAccessed)
                .Take(5)
                .ToListAsync();

           
            foreach (var recipe in recipes)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipes;
        }

        public async Task<List<Recipe>> GetRecipesByMealTypeAsync(string mealType)
        {
            if (string.IsNullOrWhiteSpace(mealType))
            {
                return new List<Recipe>();
            }
            var recipes = await _dbConnection.Table<Recipe>().Where(r => r.MealType == mealType).ToListAsync();
            
            foreach (var recipe in recipes)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipes;
        }
    }
}