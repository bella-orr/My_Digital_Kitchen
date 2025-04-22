using MyDigitalKitchen.Models;
using MyDigitalKitchen.Models.ViewModels; 
using MyDigitalKitchen; 
using System.Collections.ObjectModel; 
using System.Threading.Tasks; 
using Microsoft.Maui.Controls; 
using System.Linq; 
using System.ComponentModel; 

namespace MyDigitalKitchen;

// Page to display recipe details.
// Implements INotifyPropertyChanged for UI updates.
[QueryProperty(nameof(RecipeId), nameof(Recipe.Id))] // Get Recipe ID from navigation.
public partial class RecipeInfo : ContentPage, INotifyPropertyChanged
{
    private Recipe _currentRecipe;
    // The recipe being displayed.
    public Recipe CurrentRecipe
    {
        get => _currentRecipe;
        set
        {
            if (_currentRecipe != value)
            {
                _currentRecipe = value;
                OnPropertyChanged(nameof(CurrentRecipe));
                // Update instructions list when recipe changes.
                UpdateInstructionsDisplayList();
            }
        }
    }

    private ObservableCollection<string> _instructionsDisplayList;
    // List of instructions for UI binding.
    public ObservableCollection<string> InstructionsDisplayList
    {
        get => _instructionsDisplayList;
        set
        {
            if (_instructionsDisplayList != value)
            {
                _instructionsDisplayList = value;
                OnPropertyChanged(nameof(InstructionsDisplayList));
            }
        }
    }

    // Repository for data access.
    private readonly RecipeRepository _recipeRepository;

    // Recipe ID received from navigation.
    public int RecipeId { get; set; }

    // Constructor, receives repository via DI.
    public RecipeInfo(RecipeRepository recipeRepository)
    {
        InitializeComponent();
        _recipeRepository = recipeRepository;
    }

    // Loads recipe data when navigating to the page.
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Check for valid Recipe ID.
        if (RecipeId > 0)
        {
            // Load recipe from database.
            CurrentRecipe = await _recipeRepository.GetRecipeByIdAsync(RecipeId);

            // Set page's BindingContext.
            BindingContext = this;

            // Update last accessed and save.
            if (CurrentRecipe != null)
            {
                 CurrentRecipe.LastAccessed = DateTime.Now;
                 await _recipeRepository.UpdateRecipeAsync(CurrentRecipe);
            }
        }
        else
        {
            // Go back if no valid ID.
            Console.WriteLine("RecipeInfo navigated to without valid ID.");
            await Shell.Current.GoToAsync("..");
        }
    }

    // Splits directions string for list display.
    private void UpdateInstructionsDisplayList()
    {
        if (CurrentRecipe != null && !string.IsNullOrEmpty(CurrentRecipe.Directions))
        {
            // Split directions into steps.
            var instructions = CurrentRecipe.Directions
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(step => step.Trim())
                .Where(step => !string.IsNullOrWhiteSpace(step))
                .ToList();
            InstructionsDisplayList = new ObservableCollection<string>(instructions);
        }
        else
        {
            InstructionsDisplayList = new ObservableCollection<string>();
        }
    }

    // Notify UI of property changes.
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Handles Edit button click.
    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        if (CurrentRecipe != null)
        {
             // Navigate to Edit page, pass recipe ID.
             await Shell.Current.GoToAsync($"{nameof(EditPage)}?{nameof(Recipe.Id)}={CurrentRecipe.Id}");
        }
    }

    // Handles Favorite/Unfavorite button click.
    private async void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        if (CurrentRecipe != null)
        {
            // Toggle favorite status.
            CurrentRecipe.IsFavorite = !CurrentRecipe.IsFavorite;

            // Save updated recipe.
            await _recipeRepository.UpdateRecipeAsync(CurrentRecipe);

            // UI updates via binding.
        }
    }

    // Handles Delete button click.
    private async void deleteButton_Clicked(object sender, EventArgs e)
    {
        if (CurrentRecipe != null)
        {
            // Ask for confirmation.
            bool confirmDelete = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete '{CurrentRecipe.Title}'?", "Yes", "No");

            if (confirmDelete)
            {
                // Delete recipe from database.
                await _recipeRepository.DeleteRecipeAsync(CurrentRecipe.Id);

                // Navigate back.
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}