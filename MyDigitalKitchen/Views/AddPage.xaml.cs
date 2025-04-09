using MyDigitalKitchen.Models;

namespace MyDigitalKitchen.Views;

public partial class AddPage : ContentPage
{
    public AddPage()
    {
        InitializeComponent();
    }

    private void saveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RecipeNameEntry.Text) ||
            string.IsNullOrWhiteSpace(CategoryEntry.Text) ||
            string.IsNullOrWhiteSpace(IngredientsEditor.Text) ||
            string.IsNullOrWhiteSpace(InstructionsEditor.Text))
        {
            outputLabel.Text = "Please fill in all fields.";
            outputLabel.TextColor = Colors.Red;
            outputLabel.IsVisible = true;
            return;
        }

        var newRecipe = new Recipe
        {
            Title = RecipeNameEntry.Text,
            MealType = CategoryEntry.Text,
            Ingredients = IngredientsEditor.Text.Split(',').Select(i => i.Trim()).ToList(),
            Directions = InstructionsEditor.Text,
            LastAccessed = DateTime.Now
        };

        RecipeRepository.Instance.AddRecipe(newRecipe); // Use the Singleton

        outputLabel.Text = "Recipe Saved!";
        outputLabel.TextColor = Colors.Green;
        outputLabel.IsVisible = true;

        RecipeNameEntry.Text = "";
        CategoryEntry.Text = "";
        IngredientsEditor.Text = "";
        InstructionsEditor.Text = "";
    }
}