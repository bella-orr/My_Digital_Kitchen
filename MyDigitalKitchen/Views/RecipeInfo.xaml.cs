using MyDigitalKitchen.Models;

namespace MyDigitalKitchen;

public partial class RecipeInfo : ContentPage
{

    public Recipe CurrentRecipe { get; set; } 
    public RecipeInfo(Recipe recipe)
    {
        InitializeComponent();
        CurrentRecipe = recipe;
        BindingContext = this; // Set the BindingContext to this page
        UpdateUI();
    }

    public RecipeInfo() // Default constructor (might be needed by the navigation system)
    {
        InitializeComponent();
    }

    private void UpdateUI()
    {
        if (CurrentRecipe != null)
        {
            dishTitle.Text = CurrentRecipe.Title;
            // You'll need to bind your ingredients and instructions CollectionViews here
            // For now, let's just set some placeholder text
            ingredientsList.ItemsSource = CurrentRecipe.Ingredients;
            InstructionsList.ItemsSource = new List<string> { CurrentRecipe.Directions };
        }
    }
}