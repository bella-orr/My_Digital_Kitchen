using MyDigitalKitchen.Models;

namespace MyDigitalKitchen.Views;

public partial class EditPage : ContentPage
{
    private Recipe _editingRecipe;
	public EditPage(Recipe recipeToEdit)
	{
		InitializeComponent();

        _editingRecipe = recipeToEdit;

        RecipeNameEntry.Text = _editingRecipe.Title;
        CategoryEntry.Text = _editingRecipe.MealType;
        IngredientsEditor.Text = string.Join(", ", _editingRecipe.Ingredients);
        InstructionsEditor.Text = _editingRecipe.Directions;
    }

    private void updateButton_Clicked(object sender, EventArgs e)
    {
        //checks to see that the category and title are filled 
        if (string.IsNullOrEmpty(RecipeNameEntry.Text) || string.IsNullOrEmpty(CategoryEntry.Text)) 
        {
            outputLabel.Text = "Please make sure that the Title and Category are filled";
            outputLabel.IsVisible = true;
            return;
        }

        //if everything is fille out, it will update the recipe
        _editingRecipe.Title = RecipeNameEntry.Text;
        _editingRecipe.MealType = CategoryEntry.Text;
        _editingRecipe.Ingredients = IngredientsEditor.Text
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.Trim())
            .ToList();
        _editingRecipe.Directions = InstructionsEditor.Text;
        _editingRecipe.LastAccessed = DateTime.Now;

        RecipeRepository.Instance.UpdateRecipe(_editingRecipe);


        //ouputs to user that the recipe was saved
        outputLabel.Text = "Recipe updated!";
        outputLabel.IsVisible = true;
    }
}