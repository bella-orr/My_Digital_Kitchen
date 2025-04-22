using MyDigitalKitchen.Models;
using MyDigitalKitchen; 

namespace MyDigitalKitchen.Views;

public partial class AddPage : ContentPage
{
    private readonly RecipeRepository _recipeRepository;

    
    public AddPage(RecipeRepository recipeRepository)
    {
        InitializeComponent();
        _recipeRepository = recipeRepository;
    }

    
    private async void saveButton_Clicked(object sender, EventArgs e)
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
            
            Ingredients = IngredientsEditor.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToList(),
            Directions = InstructionsEditor.Text,
            LastAccessed = DateTime.Now 
        };

        
        await _recipeRepository.AddRecipeAsync(newRecipe);

        
        outputLabel.Text = "Recipe Saved!";
        outputLabel.TextColor = Colors.Green;
        outputLabel.IsVisible = true;

        
        RecipeNameEntry.Text = "";
        CategoryEntry.Text = "";
        IngredientsEditor.Text = "";
        InstructionsEditor.Text = "";

    }
}