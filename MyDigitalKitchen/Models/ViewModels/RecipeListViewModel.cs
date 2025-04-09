using MyDigitalKitchen.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyDigitalKitchen.Models.ViewModels
{
    public class RecipeListViewModel
    {
        public ObservableCollection<RecipeGroup> GroupedRecipes { get; set; }

        public RecipeListViewModel()
        {
            GroupedRecipes = new ObservableCollection<RecipeGroup>();
            LoadAndGroupRecipes();
        }

        private void LoadAndGroupRecipes()
        {
            var allRecipes = RecipeRepository.Instance.GetAllRecipes(); // Get recipes from the repository
            GroupedRecipes.Clear();

            if (allRecipes != null && allRecipes.Any())
            {
                var grouped = allRecipes
                    .OrderBy(r => r.Title)
                    .GroupBy(r =>
                    {
                        if (string.IsNullOrEmpty(r.Title))
                        {
                            return "#";
                        }
                        char firstChar = r.Title[0];
                        if (char.IsLetter(firstChar))
                        {
                            return firstChar.ToString().ToUpper();
                        }
                        else if (char.IsDigit(firstChar))
                        {
                            return "#";
                        }
                        else
                        {
                            return "#";
                        }
                    })
                    .Select(g => new RecipeGroup(g.Key, g.ToList()))
                    .OrderBy(g => g.Letter == "#" ? 27 : g.Letter[0]);

                foreach (var group in grouped)
                {
                    GroupedRecipes.Add(group);
                }
            }
        }
    }
}