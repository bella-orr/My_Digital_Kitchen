using MyDigitalKitchen.Views;
using MyDigitalKitchen.Models;
using MyDigitalKitchen.Models.ViewModels;

namespace MyDigitalKitchen
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            LoadRecipes();

            FavoriteRecipiesList.SelectionChanged += OnRecipeSelected;
            RecentRecipiesList.SelectionChanged += OnRecipeSelected;
        }


        //Loads the recipes to the respective sections
        private void LoadRecipes() 
        {
            var allRecipes = RecipeRepository.Instance.GetAllRecipes();

            //Loads the favorites
            var favRecipes = allRecipes
                .Where(r => r.IsFavorite)
                .OrderBy(r => r.Title)
                .ToList();

            FavoriteRecipiesList.ItemsSource = favRecipes;

            //Loads the recently accessed recipes
            var recentRecipes = RecipeRepository.Instance.GetRecentlyAccessed();
            RecentRecipiesList.ItemsSource = recentRecipes;

        }

        //checks to see if a recipe was selected
        private void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Recipe selectedRecipe)
            {
                // Updates the LastAccessed property when the recipe is accessed
                selectedRecipe.LastAccessed = DateTime.Now;

                // Updates the recipe in the repository to save the change
                RecipeRepository.Instance.UpdateRecipe(selectedRecipe);

              


                // Navigates to the recipe details page
                Navigation.PushAsync(new RecipeInfo(selectedRecipe));
               
            }
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Search", "Search options will be added later", "OK");
        }

        private void nextButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RecipeList());
        }

        private void addButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPage());
        }

        //forces the main page to reload withe the correct information
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadRecipes();
        }
    }

}
