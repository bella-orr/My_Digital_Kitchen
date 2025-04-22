using System.Collections.ObjectModel;
using MyDigitalKitchen.Models;
using MyDigitalKitchen.Models.ViewModels;
using MyDigitalKitchen.Views;
using MyDigitalKitchen;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Linq;

namespace MyDigitalKitchen;

public partial class RecipeList : ContentPage
{
    private readonly RecipeListViewModel _viewModel; 
    private readonly RecipeRepository _recipeRepository;

   
    public RecipeList(RecipeListViewModel viewModel, RecipeRepository recipeRepository)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _recipeRepository = recipeRepository; 
        BindingContext = _viewModel; 
    }

   
    private async void FilterButton_Clicked(object sender, EventArgs e)
    {
        
        string action = await DisplayActionSheet("Filter Recipes By:", "Cancel", null,
            "All Recipes",
            "Favorites",
            "Meal Type"
        
        );

        List<Recipe> filteredRecipes = new List<Recipe>();

        switch (action)
        {
            case "All Recipes":
                filteredRecipes = await _recipeRepository.GetAllRecipesAsync();
                break;

            case "Favorites":
                filteredRecipes = await _recipeRepository.GetFavoritesAsync();
                break;

            case "Meal Type":
                
                var allRecipesForMealTypes = await _recipeRepository.GetAllRecipesAsync();
                var availableMealTypes = allRecipesForMealTypes
                                         .Select(r => r.MealType)
                                         .Where(mt => !string.IsNullOrWhiteSpace(mt))
                                         .Distinct()
                                         .OrderBy(mt => mt)
                                         .ToList();

                if (availableMealTypes.Any())
                {
                   
                    availableMealTypes.Insert(0, "All Meal Types");

                    string mealTypeAction = await DisplayActionSheet("Select Meal Type:", "Cancel", null,
                        availableMealTypes.ToArray()
                    );

                    if (mealTypeAction == "All Meal Types")
                    {
                        filteredRecipes = await _recipeRepository.GetAllRecipesAsync();
                    }
                    else if (!string.IsNullOrEmpty(mealTypeAction) && mealTypeAction != "Cancel")
                    {
                        filteredRecipes = await _recipeRepository.GetRecipesByMealTypeAsync(mealTypeAction);
                    }
                   
                }
                else
                {
                    await DisplayAlert("Info", "No meal types available for filtering.", "OK");
                    
                    filteredRecipes = await _recipeRepository.GetAllRecipesAsync(); 
                }
                break;

            default:
                
                filteredRecipes = await _recipeRepository.GetAllRecipesAsync(); 
                break;
        }

        
        _viewModel.GroupRecipes(filteredRecipes);
    }

   
    private async void Recipe_Selected(object sender, SelectionChangedEventArgs e)
    {
        var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;
        if (selectedRecipe != null)
        {
            
            await Shell.Current.GoToAsync($"{nameof(RecipeInfo)}?{nameof(Recipe.Id)}={selectedRecipe.Id}");

           
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

       
        LetterList.SelectedItem = null;

        
        var allRecipes = await _recipeRepository.GetAllRecipesAsync();
        _viewModel.GroupRecipes(allRecipes);
    }
}