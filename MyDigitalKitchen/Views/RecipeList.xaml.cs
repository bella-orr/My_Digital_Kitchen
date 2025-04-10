using System.Collections.ObjectModel;
using MyDigitalKitchen.Models;
using MyDigitalKitchen.Models.ViewModels;

namespace MyDigitalKitchen;

public partial class RecipeList : ContentPage
{
    public RecipeList()
    {
        InitializeComponent();
     
        BindingContext = new RecipeListViewModel();
    }

    private void LetterList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is RecipeGroup selectedGroup)
        {
            if (selectedGroup.Recipes != null && selectedGroup.Recipes.Any())
            {
                Recipe selectedRecipe = selectedGroup.Recipes.FirstOrDefault();
                if (selectedRecipe != null)
                {
                    Navigation.PushAsync(new RecipeInfo(selectedRecipe));
                    ((CollectionView)sender).SelectedItem = null;
                }
            }
        }
        else if (e.SelectedItem is Recipe selectedRecipe)
        {
            Navigation.PushAsync(new RecipeInfo(selectedRecipe));
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void FilterButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Filter", "Filter options will be added later", "OK");
    }

    private void nextButton2_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RecipeInfo());
    }

    private void Recipe_Selected(object sender, SelectionChangedEventArgs e) 
    {
        var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;
        if (selectedRecipe != null) 
        {
            Navigation.PushAsync(new RecipeInfo(selectedRecipe));

            // Deselect the item after navigation
            ((CollectionView)sender).SelectedItem = null; 
        }

        
    }

    //this will aloow the user to select the same recipe again if they toggle backwards. It clears the selction of the collection group of the letterList
    protected override void OnAppearing()
    {
        base.OnAppearing();

        //resets the selected item in the outer CollectionView
        LetterList.SelectedItem = null;

        BindingContext = new RecipeListViewModel();
    }

    
}