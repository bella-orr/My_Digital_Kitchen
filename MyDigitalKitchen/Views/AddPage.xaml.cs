using MyDigitalKitchen.Models;

namespace MyDigitalKitchen.Views;

public partial class AddPage : ContentPage
{
    public AddPage()
    {
        InitializeComponent();
    }

    //When the user clicks the save button, this method is called
    private void saveButton_Clicked(object sender, EventArgs e)
    {
        // Check if any of the fields are empty
        if (string.IsNullOrWhiteSpace(RecipeNameEntry.Text) ||
            string.IsNullOrWhiteSpace(CategoryEntry.Text) ||
            string.IsNullOrWhiteSpace(IngredientsEditor.Text) ||
            string.IsNullOrWhiteSpace(InstructionsEditor.Text))
        {
            // Display an error message if they are empty
            outputLabel.Text = "Please fill in all fields.";
            outputLabel.TextColor = Colors.Red;
            outputLabel.IsVisible = true;
            return;
        }

        //Creates new recipe if the fields are not empty
        var newRecipe = new Recipe
        {
            Title = RecipeNameEntry.Text,
            MealType = CategoryEntry.Text,
            Ingredients = IngredientsEditor.Text.Split(',').Select(i => i.Trim()).ToList(),
            Directions = InstructionsEditor.Text,
            LastAccessed = DateTime.Now
        };

        RecipeRepository.Instance.AddRecipe(newRecipe);

        // Display a success message
        outputLabel.Text = "Recipe Saved!";
        outputLabel.TextColor = Colors.Green;
        outputLabel.IsVisible = true;

        //sets the fields to empty after saving
        RecipeNameEntry.Text = "";
        CategoryEntry.Text = "";
        IngredientsEditor.Text = "";
        InstructionsEditor.Text = "";
    }
}