using MyDigitalKitchen.Views;
using MyDigitalKitchen.Models;
using MyDigitalKitchen.Models.ViewModels;
using MyDigitalKitchen; // Namespace for RecipeRepository
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Maui.Controls; // Needed for Shell navigation and Page

namespace MyDigitalKitchen
{
    // Code-behind for the main app page.
    public partial class MainPage : ContentPage
    {
        // Repository for data access.
        private readonly RecipeRepository _recipeRepository;

        // Constructor, receives repository via DI.
        public MainPage(RecipeRepository recipeRepository)
        {
            InitializeComponent();
            _recipeRepository = recipeRepository;

            // Subscribe to list selection events.
            FavoriteRecipiesList.SelectionChanged += OnRecipeSelected;
            RecentRecipiesList.SelectionChanged += OnRecipeSelected;
        }

        // Load recent and favorite recipes.
        private async Task LoadRecipesAsync()
        {
            var allRecipes = await _recipeRepository.GetAllRecipesAsync();

            // Filter and set favorites.
            var favRecipes = allRecipes
                .Where(r => r.IsFavorite)
                .OrderBy(r => r.Title)
                .ToList();
            FavoriteRecipiesList.ItemsSource = favRecipes;

            // Fetch and set recent recipes.
            var recentRecipes = await _recipeRepository.GetRecentlyAccessedAsync();
            RecentRecipiesList.ItemsSource = recentRecipes;
        }

        // Handle recipe selection from lists.
        private async void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Recipe selectedRecipe)
            {
                // Update last accessed time.
                selectedRecipe.LastAccessed = DateTime.Now;

                // Save updated recipe.
                await _recipeRepository.UpdateRecipeAsync(selectedRecipe);

                // Navigate to recipe info page.
                await Shell.Current.GoToAsync($"{nameof(RecipeInfo)}?{nameof(Recipe.Id)}={selectedRecipe.Id}");

                // Deselect item.
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        // Handle search button click.
        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            string search = SearchEntry.Text?.Trim().ToLower();

            // Reload all if search is empty.
            if (string.IsNullOrWhiteSpace(search))
            {
                await LoadRecipesAsync();
                return;
            }

            // Search and display results in recents list.
            var matchingRecipes = await _recipeRepository.SearchRecipesAsync(search);
            var orderedMatchingRecipes = matchingRecipes.OrderByDescending(r => r.LastAccessed).ToList();
            RecentRecipiesList.ItemsSource = orderedMatchingRecipes;
        }

        // Navigate to the Recipe List page.
        private async void nextButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(RecipeList));
        }

        // Navigate to the Add Recipe page.
        private async void addButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddPage));
        }

        // Called when the page becomes visible.
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadRecipesAsync(); // Reload data.
        }
    }
}