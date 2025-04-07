
using System.Collections.ObjectModel;

namespace MyDigitalKitchen;

public partial class RecipeList : ContentPage
{



	public RecipeList()
	{
		InitializeComponent();

        //Ensures ViewModel is set
        BindingContext = new RecipeListViewModel(); 

    }

    private void FilterButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Filter", "Filter options will be added later", "OK");
    }

    private void nextButton2_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RecipeInfo());
    }




}