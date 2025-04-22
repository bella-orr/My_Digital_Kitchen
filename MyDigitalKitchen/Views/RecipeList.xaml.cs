using System.Collections.ObjectModel;
using MyDigitalKitchen.Models;
using MyDigitalKitchen.Models.ViewModels;
using MyDigitalKitchen.Views;
using MyDigitalKitchen; // For RecipeRepository
using System.Threading.Tasks;
using Microsoft.Maui.Controls; // For Page, Shell, DisplayActionSheet, DisplayAlert
using System.Linq; // For LINQ

namespace MyDigitalKitchen;

// Page to display the list of recipes.
public partial class RecipeList : ContentPage
{
    private readonly RecipeListViewModel _viewModel;
    private readonly RecipeRepository _recipeRepository;

    // Constructor, receives ViewModel and Repository via DI.
    public RecipeList(RecipeListViewModel viewModel, RecipeRepository recipeRepository)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _recipeRepository = recipeRepository;
        BindingContext = _viewModel;
    }

    // Handles Filter button click.
    private async void FilterButton_Clicked(object sender, EventArgs e)
    {
        // Show filter options.
        string action = await DisplayActionSheet("Filter Recipes By:", "Cancel", null,
            "All Recipes",
            "Favorites",
            "Meal Type"
        );

        List<Recipe> filteredRecipes = new List<Recipe>();

        // Handle filter selection.
        switch (action)
        {
            case "All Recipes":
                filteredRecipes = await _recipeRepository.GetAllRecipesAsync();
                break;

            case "Favorites":
                filteredRecipes = await _recipeRepository.GetFavoritesAsync();
                break;

            case "Meal Type":
                // Get available meal types and show sub-filter.
                var allRecipesForMealTypes = await _recipeRepository.GetAllRecipesAsync();
                var availableMealTypes = allRecipesForMealTypes
                                         .Select(r => r.MealType)
                                         .Where(mt => !string.IsNullOrWhiteSpace(mt))
                                         .Distinct()
                                         .OrderBy(mt => mt)
                                         .ToList();

                if (availableMealTypes.Any())
                {
                    // Add option for all meal types.
                    availableMealTypes.Insert(0, "All Meal Types");

                    string mealTypeAction = await DisplayActionSheet("Select Meal Type:", "Cancel", null,
                        availableMealTypes.ToArray()
                    );

                    // Filter by selected meal type or get all.
                    if (mealTypeAction == "All Meal Types")
                    {
                        filteredRecipes = await _recipeRepository.GetAllRecipesAsync();
                    }
                    else if (!string.IsNullOrEmpty(mealTypeAction) && mealTypeAction != "Cancel")
                    {
                        filteredRecipes = await _recipeRepository.GetRecipesByMealTypeAsync(mealTypeAction);
                    }
                    // If cancelled, filteredRecipes remains empty.
                }
                else
                {
                    await DisplayAlert("Info", "No meal types available.", "OK");
                    // Show all recipes if no meal types exist.
                    filteredRecipes = await _recipeRepository.GetAllRecipesAsync();
                }
                break;

            default:
                // Reload all on cancel or unhandled action.
                filteredRecipes = await _recipeRepository.GetAllRecipesAsync();
                break;
        }

        // Update the displayed grouped list in the ViewModel.
        _viewModel.GroupRecipes(filteredRecipes);
    }

    // Handles recipe selection in the list.
    private async void Recipe_Selected(object sender, SelectionChangedEventArgs e)
    {
        var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;
        if (selectedRecipe != null)
        {
            // Navigate to Recipe Info page.
            await Shell.Current.GoToAsync($"{nameof(RecipeInfo)}?{nameof(Recipe.Id)}={selectedRecipe.Id}");

            // Deselect item.
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    // Called when the page appears.
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Deselect item in main list.
        LetterList.SelectedItem = null;

        // Load and group all recipes initially.
        var allRecipes = await _recipeRepository.GetAllRecipesAsync();
        _viewModel.GroupRecipes(allRecipes);
    }
}