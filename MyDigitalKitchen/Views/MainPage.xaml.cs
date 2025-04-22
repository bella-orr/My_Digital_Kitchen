using MyDigitalKitchen.Views;
using MyDigitalKitchen.Models;
using MyDigitalKitchen.Models.ViewModels;
using MyDigitalKitchen;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Maui.Controls; 

namespace MyDigitalKitchen
{
    public partial class MainPage : ContentPage
    {
        private readonly RecipeRepository _recipeRepository;

       
        public MainPage(RecipeRepository recipeRepository)
        {
            InitializeComponent();
            _recipeRepository = recipeRepository;

            FavoriteRecipiesList.SelectionChanged += OnRecipeSelected;
            RecentRecipiesList.SelectionChanged += OnRecipeSelected;
        }

        
        private async Task LoadRecipesAsync()
        {
            var allRecipes = await _recipeRepository.GetAllRecipesAsync();

            var favRecipes = allRecipes
            .Where(r => r.IsFavorite)
            .OrderBy(r => r.Title)
            .ToList();

            FavoriteRecipiesList.ItemsSource = favRecipes;

            var recentRecipes = await _recipeRepository.GetRecentlyAccessedAsync();
            RecentRecipiesList.ItemsSource = recentRecipes;
        }

       
        private async void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Recipe selectedRecipe)
            {
               
                selectedRecipe.LastAccessed = DateTime.Now;

               
                await _recipeRepository.UpdateRecipeAsync(selectedRecipe);

                
                await Shell.Current.GoToAsync($"{nameof(RecipeInfo)}?{nameof(Recipe.Id)}={selectedRecipe.Id}");
                

               
                ((CollectionView)sender).SelectedItem = null;
            }
        }

       
        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            string search = SearchEntry.Text?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(search))
            {
                await LoadRecipesAsync();
                return;
            }

            var matchingRecipes = await _recipeRepository.SearchRecipesAsync(search);
            var orderedMatchingRecipes = matchingRecipes.OrderByDescending(r => r.LastAccessed).ToList();
            RecentRecipiesList.ItemsSource = orderedMatchingRecipes;
        }

        
        private async void nextButton_Clicked(object sender, EventArgs e)
        {

            await Shell.Current.GoToAsync(nameof(RecipeList));
 
        }


        private async void addButton_Clicked(object sender, EventArgs e)
        {

            await Shell.Current.GoToAsync(nameof(AddPage));
           
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadRecipesAsync();
        }
    }
}
