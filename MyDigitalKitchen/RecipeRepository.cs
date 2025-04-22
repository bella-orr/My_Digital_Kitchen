using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyDigitalKitchen.Models;
using MyDigitalKitchen.Services;
using SQLite;

namespace MyDigitalKitchen
{
    // Handles data access operations for Recipes using SQLite.
    public class RecipeRepository
    {
        // Connection to the SQLite database.
        private readonly SQLiteAsyncConnection _dbConnection;

        // Constructor, receives DatabaseService via DI to get the connection.
        public RecipeRepository(DatabaseService databaseService)
        {
            _dbConnection = databaseService.GetConnection();
        }

        // --- Manual Serialization/Deserialization for Ingredients ---

        // Converts a List<string> of ingredients to a single string for storage.
        private string SerializeIngredients(List<string> ingredients)
        {
            if (ingredients == null || !ingredients.Any())
                return null;
            // Joins list items into a string, escaping commas.
            return string.Join(",", ingredients.Select(item => item.Replace(",", "&#44;")));
        }

        // Converts a stored ingredient string back to a List<string>.
        private List<string> DeserializeIngredients(string ingredientsBlob)
        {
            if (string.IsNullOrEmpty(ingredientsBlob))
                return new List<string>();
            // Splits string by commas, unescapes commas, and trims whitespace.
            return ingredientsBlob.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(item => item.Replace("&#44;", ",").Trim())
                                  .ToList();
        }

        // --- CRUD and Query Methods ---

        // Gets all recipes from the database.
        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            var recipes = await _dbConnection.Table<Recipe>().ToListAsync();
            // Deserialize ingredients after loading.
            foreach (var recipe in recipes)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipes;
        }

        // Gets a single recipe by its ID.
        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            var recipe = await _dbConnection.Table<Recipe>().Where(r => r.Id == id).FirstOrDefaultAsync();
            // Deserialize ingredients if recipe found.
            if (recipe != null)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipe;
        }

        // Adds a new recipe to the database.
        public async Task AddRecipeAsync(Recipe recipe)
        {
            // Serialize ingredients before saving.
            recipe.IngredientsBlob = SerializeIngredients(recipe.Ingredients);
            recipe.LastAccessed = DateTime.Now; // Set last accessed on creation.
            await _dbConnection.InsertAsync(recipe);
        }

        // Updates an existing recipe in the database.
        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            // Serialize ingredients before saving.
            recipe.IngredientsBlob = SerializeIngredients(recipe.Ingredients);
            await _dbConnection.UpdateAsync(recipe);
        }

        // Deletes a recipe from the database by its ID.
        public async Task DeleteRecipeAsync(int id)
        {
            await _dbConnection.DeleteAsync<Recipe>(id);
        }

        // Searches recipes by title or ingredients.
        public async Task<List<Recipe>> SearchRecipesAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetAllRecipesAsync(); // Return all if keyword is empty.
            }

            // Search in Title and the serialized IngredientsBlob string.
            var results = await _dbConnection.Table<Recipe>()
                .Where(r => (!string.IsNullOrEmpty(r.Title) && r.Title.ToLower().Contains(keyword.ToLower())) ||
                            (!string.IsNullOrEmpty(r.IngredientsBlob) && r.IngredientsBlob.ToLower().Contains(keyword.ToLower())))
                .ToListAsync();

            // Deserialize ingredients for search results.
             foreach (var recipe in results)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }

            return results;
        }

        // Gets all recipes marked as favorite.
        public async Task<List<Recipe>> GetFavoritesAsync()
        {
            var recipes = await _dbConnection.Table<Recipe>().Where(r => r.IsFavorite).ToListAsync();
             // Deserialize ingredients for favorites.
             foreach (var recipe in recipes)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipes;
        }

        // Gets the most recently accessed recipes (top 5).
        public async Task<List<Recipe>> GetRecentlyAccessedAsync()
        {
            var recipes = await _dbConnection.Table<Recipe>()
                .OrderByDescending(r => r.LastAccessed)
                .Take(5)
                .ToListAsync();

            // Deserialize ingredients for recent recipes.
            foreach (var recipe in recipes)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipes;
        }

        // Gets recipes filtered by meal type.
        public async Task<List<Recipe>> GetRecipesByMealTypeAsync(string mealType)
        {
             if (string.IsNullOrWhiteSpace(mealType))
             {
                 return new List<Recipe>(); // Return empty if meal type is not specified.
             }
             var recipes = await _dbConnection.Table<Recipe>().Where(r => r.MealType == mealType).ToListAsync();
             // Deserialize ingredients for meal type results.
             foreach (var recipe in recipes)
            {
                recipe.Ingredients = DeserializeIngredients(recipe.IngredientsBlob);
            }
            return recipes;
        }
    }
}