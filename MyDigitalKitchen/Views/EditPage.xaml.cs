using MyDigitalKitchen.Models;
using MyDigitalKitchen;
using Microsoft.Maui.Controls;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace MyDigitalKitchen.Views;

// Page for editing a recipe.
[QueryProperty(nameof(RecipeId), nameof(Recipe.Id))] // Receives Recipe ID via navigation.
public partial class EditPage : ContentPage
{
    private Recipe _editingRecipe;
    private readonly RecipeRepository _recipeRepository;

    // Recipe ID received from previous page.
    public int RecipeId { get; set; }

    // Constructor, receives repository via DI.
    public EditPage(RecipeRepository recipeRepository)
    {
        InitializeComponent();
        _recipeRepository = recipeRepository;
    }

    // Loads recipe data when navigating to the page.
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (RecipeId > 0)
        {
            // Load recipe from database.
            _editingRecipe = await _recipeRepository.GetRecipeByIdAsync(RecipeId);

            // Populate UI fields.
            if (_editingRecipe != null)
            {
                RecipeNameEntry.Text = _editingRecipe.Title;
                CategoryEntry.Text = _editingRecipe.MealType;
                IngredientsEditor.Text = string.Join(", ", _editingRecipe.Ingredients);
                InstructionsEditor.Text = _editingRecipe.Directions;
            }
            else
            {
                // Go back if recipe not found.
                Console.WriteLine($"Recipe with ID {RecipeId} not found.");
                await Shell.Current.GoToAsync("..");
            }
        }
        else
        {
            // Go back if no valid ID was passed.
            Console.WriteLine("EditPage navigated to without valid ID.");
            await Shell.Current.GoToAsync("..");
        }
    }

    // Handles Save button click.
    private async void updateButton_Clicked(object sender, EventArgs e)
    {
        // Validate input.
        if (string.IsNullOrEmpty(RecipeNameEntry.Text) || string.IsNullOrEmpty(CategoryEntry.Text))
        {
            outputLabel.Text = "Fill Title and Category.";
            outputLabel.IsVisible = true;
            outputLabel.TextColor = Colors.Red;
            return;
        }

        // Check if recipe was loaded.
        if (_editingRecipe == null)
        {
            outputLabel.Text = "Error: Recipe not loaded.";
            outputLabel.IsVisible = true;
            outputLabel.TextColor = Colors.Red;
            return;
        }

        // Update recipe object from UI.
        _editingRecipe.Title = RecipeNameEntry.Text;
        _editingRecipe.MealType = CategoryEntry.Text;
        _editingRecipe.Ingredients = IngredientsEditor.Text
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.Trim())
            .ToList();
        _editingRecipe.Directions = InstructionsEditor.Text;
        _editingRecipe.LastAccessed = DateTime.Now;

        // Save changes to database.
        await _recipeRepository.UpdateRecipeAsync(_editingRecipe);

        // Show success message.
        outputLabel.Text = "Recipe updated!";
        outputLabel.IsVisible = true;
        outputLabel.TextColor = Colors.Green;

        // Optional: Navigate back.
        // await Shell.Current.GoToAsync("..");
    }
}