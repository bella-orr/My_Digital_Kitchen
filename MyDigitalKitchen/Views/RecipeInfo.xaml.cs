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
            InstructionsList.ItemsSource = new List<string> { CurrentRecipe.Directions };
        }
    }
}