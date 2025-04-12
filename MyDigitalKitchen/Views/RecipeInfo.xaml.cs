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

    //Updates the UI with the current recipe's details
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

            FavoriteButton.Text = CurrentRecipe.IsFavorite ? "Unfavorite" : "Favorite ";
        }
    }

    //Set up for the Edit button which will be implemented later
    private void EditButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Edit", "Edit options will be added later", "OK");
    }

    //Allows the user to favorite the current recipe that is saved
    private void FavoriteButton_Clicked(object sender, EventArgs e)
    {

        if (CurrentRecipe != null) 
        {
            CurrentRecipe.IsFavorite = !CurrentRecipe.IsFavorite;

            //will be needed to save the updated recipe  status to the app's storage data
            //RecipeRepository.Instance.UpdateRecipe(CurrentRecipe);

            FavoriteButton.Text = CurrentRecipe.IsFavorite ? "Unfavorite" : "Favorite ";

           
        }

    }
}