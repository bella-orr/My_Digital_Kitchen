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
        if (e.CurrentSelection.FirstOrDefault() is Recipe selectedRecipe)
        {
            Navigation.PushAsync(new RecipeInfo(selectedRecipe));
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void MealTypePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}