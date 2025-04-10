using MyDigitalKitchen.Models;

namespace MyDigitalKitchen;

public partial class RecipeInfo : ContentPage
{

    public Recipe CurrentRecipe { get; set; } 
    public RecipeInfo(Recipe recipe)
    {
        InitializeComponent();
        CurrentRecipe = recipe;
        BindingContext = this; 
        UpdateUI();
    }

    public RecipeInfo() 
    {
        InitializeComponent();
    }

    private void UpdateUI()
    {
        if (CurrentRecipe != null)
        {
            dishTitle.Text = CurrentRecipe.Title;
        

            ingredientsList.ItemsSource = CurrentRecipe.Ingredients;

            // Split the directions string into steps
            var instructions = CurrentRecipe.Directions
                .Split(new[] { '\n', '\r', ',' , '.' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(step => step.Trim()) // removes spaces before/after each step
                .Where(step => !string.IsNullOrWhiteSpace(step))
                .ToList();

            InstructionsList.ItemsSource = instructions;
        }
    }
}