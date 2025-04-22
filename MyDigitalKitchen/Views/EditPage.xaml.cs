using MyDigitalKitchen.Models;
using MyDigitalKitchen;
using Microsoft.Maui.Controls;
using System.Linq;

namespace MyDigitalKitchen.Views;


[QueryProperty(nameof(RecipeId), nameof(Recipe.Id))]
public partial class EditPage : ContentPage
{
    private Recipe _editingRecipe; 
    private readonly RecipeRepository _recipeRepository;

   
    public int RecipeId { get; set; }

    
    public EditPage(RecipeRepository recipeRepository)
    {
        InitializeComponent();
        _recipeRepository = recipeRepository;
       
    }

    
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        
        if (RecipeId > 0)
        {
            
            _editingRecipe = await _recipeRepository.GetRecipeByIdAsync(RecipeId);

            
            if (_editingRecipe != null)
            {
                RecipeNameEntry.Text = _editingRecipe.Title;
                CategoryEntry.Text = _editingRecipe.MealType;
                IngredientsEditor.Text = string.Join(", ", _editingRecipe.Ingredients); 
                InstructionsEditor.Text = _editingRecipe.Directions;
            }
            else
            {
                
                Console.WriteLine($"Recipe with ID {RecipeId} not found for editing.");
                await Shell.Current.GoToAsync(".."); 
            }
        }
        else
        {
            
            Console.WriteLine("EditPage navigated to without a valid RecipeId.");
            await Shell.Current.GoToAsync(".."); 
        }
    }

  
    private async void updateButton_Clicked(object sender, EventArgs e)
    {
        
        if (string.IsNullOrEmpty(RecipeNameEntry.Text) || string.IsNullOrEmpty(CategoryEntry.Text))
        {
            outputLabel.Text = "Please make sure that the Title and Category are filled";
            outputLabel.IsVisible = true;
            outputLabel.TextColor = Colors.Red;
            return;
        }

        
        if (_editingRecipe == null)
        {
            outputLabel.Text = "Error: Recipe not loaded for editing.";
            outputLabel.IsVisible = true;
            outputLabel.TextColor = Colors.Red;
            return;
        }

        
        _editingRecipe.Title = RecipeNameEntry.Text;
        _editingRecipe.MealType = CategoryEntry.Text;
        _editingRecipe.Ingredients = IngredientsEditor.Text
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.Trim())
            .ToList();
        _editingRecipe.Directions = InstructionsEditor.Text;
        _editingRecipe.LastAccessed = DateTime.Now; 

        
        await _recipeRepository.UpdateRecipeAsync(_editingRecipe);

        
        outputLabel.Text = "Recipe updated!";
        outputLabel.IsVisible = true;
        outputLabel.TextColor = Colors.Green;


    }
}