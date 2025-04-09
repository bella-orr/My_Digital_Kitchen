using MyDigitalKitchen.Models;
using MyDigitalKitchen.Models.ViewModels;

namespace MyDigitalKitchen.Views;

public partial class AddPage : ContentPage
{
    private RecipeListViewModel _viewModel;

    public AddPage()
    {
        InitializeComponent();
      
        _viewModel = new RecipeListViewModel();
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

        _viewModel.AllRecipes.Add(newRecipe);

        
        _viewModel.GroupedRecipes.Clear();
        var grouped = _viewModel.AllRecipes
            .OrderBy(r => r.Title)
            .GroupBy(r =>
            {
                if (string.IsNullOrEmpty(r.Title))
                {
                    return "#"; // Group recipes with no title under '#'
                }
                char firstChar = r.Title[0];
                if (char.IsLetter(firstChar))
                {
                    return firstChar.ToString().ToUpper();
                }
                else if (char.IsDigit(firstChar))
                {
                    return "#"; // Group recipes starting with a digit under '#'
                }
                else
                {
                    return "#"; // Group recipes with other non-letter characters under '#'
                }
            })
            .Select(g => new RecipeGroup(g.Key, g.ToList()))
            .OrderBy(g => g.Letter);

        foreach (var group in grouped)
        {
            _viewModel.GroupedRecipes.Add(group);
            Console.WriteLine($"Group: {group.Letter}, Recipe Count: {group.Recipes.Count}"); // Logging
        }

        outputLabel.Text = "Recipe Saved!";
        outputLabel.TextColor = Colors.Green;
        outputLabel.IsVisible = true;

        RecipeNameEntry.Text = "";
        CategoryEntry.Text = "";
        IngredientsEditor.Text = "";
        InstructionsEditor.Text = "";
    }
}