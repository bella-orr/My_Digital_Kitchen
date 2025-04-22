using MyDigitalKitchen.Models;
using MyDigitalKitchen; 
using Microsoft.Maui.Controls; 
using System; 
using System.Linq; 
using System.Threading.Tasks; 

namespace MyDigitalKitchen.Views;

// This class handles the code-behind logic for the AddPage UI.
// It allows the user to input details for a new recipe and save it.
public partial class AddPage : ContentPage
{
    // Private field to hold the injected RecipeRepository instance.
    private readonly RecipeRepository _recipeRepository;

    // Constructor for the AddPage, receives dependencies via DI.
    // The constructor receives the RecipeRepository from the DI container.
    public AddPage(RecipeRepository recipeRepository)
    {
        InitializeComponent(); // Initializes the UI components defined in AddPage.xaml
        _recipeRepository = recipeRepository; // Assigns the injected repository
    }

    // Event handler for the Save button click.
    // This method is called when the user taps the Save button. It's marked async because it performs await calls.
    private async void saveButton_Clicked(object sender, EventArgs e)
    {
        // --- Input Validation ---
        // Check if any of the required input fields are empty or contain only whitespace.
        if (string.IsNullOrWhiteSpace(RecipeNameEntry.Text) ||
            string.IsNullOrWhiteSpace(CategoryEntry.Text) ||
            string.IsNullOrWhiteSpace(IngredientsEditor.Text) ||
            string.IsNullOrWhiteSpace(InstructionsEditor.Text))
        {
            // Display an error message to the user if validation fails.
            outputLabel.Text = "Please fill in all fields.";
            outputLabel.TextColor = Colors.Red; // Set text color to red for errors
            outputLabel.IsVisible = true; // Make the output label visible
            return; // Stop the method execution
        }

        // --- Create New Recipe Object ---
        // If validation passes, create a new Recipe object using data from the UI.
        var newRecipe = new Recipe
        {
            Title = RecipeNameEntry.Text,
            MealType = CategoryEntry.Text,
            // Split the ingredients text by commas, trim whitespace, and convert to a List<string>.
            // The Recipe model's IngredientsBlob property will be populated from this list
            // by the repository's AddRecipeAsync method (manual serialization).
            Ingredients = IngredientsEditor.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToList(),
            Directions = InstructionsEditor.Text,
            LastAccessed = DateTime.Now // Set the initial LastAccessed date/time
        };

        // --- Save Recipe to Database ---
        // Call the asynchronous method on the repository to add the new recipe to the database.
        await _recipeRepository.AddRecipeAsync(newRecipe);

        // --- Provide User Feedback ---
        // Display a success message after the recipe is saved.
        outputLabel.Text = "Recipe Saved!";
        outputLabel.TextColor = Colors.Green; // Set text color to green for success
        outputLabel.IsVisible = true; // Make the output label visible

        // --- Clear Input Fields ---
        // Clear the text from all input fields, preparing the form for a new entry.
        RecipeNameEntry.Text = "";
        CategoryEntry.Text = "";
        IngredientsEditor.Text = "";
        InstructionsEditor.Text = "";

        // Note: Navigation back to the previous page could be added here if desired.
        // await Shell.Current.GoToAsync("..");
    }
}