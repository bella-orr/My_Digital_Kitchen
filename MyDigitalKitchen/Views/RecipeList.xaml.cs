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

    //for the filter button, this will be implemented later
    private void FilterButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Filter", "Filter options will be added later", "OK");
    }



    //gets the recipe selected from the collection view and passes it to the RecipeInfo page
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