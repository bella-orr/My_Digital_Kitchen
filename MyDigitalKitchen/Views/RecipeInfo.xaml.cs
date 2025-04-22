using MyDigitalKitchen.Models;
using MyDigitalKitchen.Models.ViewModels;
using MyDigitalKitchen.Views;
using MyDigitalKitchen;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls; 
using System.Linq;
using System.ComponentModel; 

namespace MyDigitalKitchen;


[QueryProperty(nameof(RecipeId), nameof(Recipe.Id))]
public partial class RecipeInfo : ContentPage, INotifyPropertyChanged
{

    private Recipe _currentRecipe;
    public Recipe CurrentRecipe
    {
        get => _currentRecipe;
        set
        {
            if (_currentRecipe != value)
            {
                _currentRecipe = value;
                OnPropertyChanged(nameof(CurrentRecipe));

                UpdateInstructionsDisplayList();
            }
        }
    }


    private ObservableCollection<string> _instructionsDisplayList;
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


    private readonly RecipeRepository _recipeRepository;


    public int RecipeId { get; set; }


    public RecipeInfo(RecipeRepository recipeRepository)
    {
        InitializeComponent();
        _recipeRepository = recipeRepository;
    }



    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (RecipeId > 0)
        {

            CurrentRecipe = await _recipeRepository.GetRecipeByIdAsync(RecipeId);


            BindingContext = this;


            if (CurrentRecipe != null)
            {
                CurrentRecipe.LastAccessed = DateTime.Now;
                await _recipeRepository.UpdateRecipeAsync(CurrentRecipe);
            }
        }
        else
        {

            Console.WriteLine("RecipeInfo navigated to without a valid RecipeId.");
            await Shell.Current.GoToAsync("..");
        }
    }


    private void UpdateInstructionsDisplayList()
    {
        if (CurrentRecipe != null && !string.IsNullOrEmpty(CurrentRecipe.Directions))
        {
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


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        if (CurrentRecipe != null)
        {

            await Shell.Current.GoToAsync($"{nameof(EditPage)}?{nameof(Recipe.Id)}={CurrentRecipe.Id}");
        }
    }


    private async void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        if (CurrentRecipe != null)
        {

            if (CurrentRecipe.IsFavorite)
            {
                FavoriteButton.Text = "Favorite";
                
            }
            else
            {
                FavoriteButton.Text = "Unfavorite";
                
            }
            CurrentRecipe.IsFavorite = !CurrentRecipe.IsFavorite;

            await _recipeRepository.UpdateRecipeAsync(CurrentRecipe);


        }
    }


    private async void deleteButton_Clicked(object sender, EventArgs e)
    {
        if (CurrentRecipe != null)
        {
            bool confirmDelete = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete '{CurrentRecipe.Title}'?", "Yes", "No");

            if (confirmDelete)
            {
                await _recipeRepository.DeleteRecipeAsync(CurrentRecipe.Id);


                await Shell.Current.GoToAsync("..");
            }
        }
    }
}