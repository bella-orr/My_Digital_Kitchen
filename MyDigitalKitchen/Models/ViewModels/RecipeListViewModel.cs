using MyDigitalKitchen.Models;
using System.Collections.ObjectModel;
using System.Linq;
using MyDigitalKitchen.Helpers; // Needed for the ToObservableCollection extension method (although not directly used in this snippet)
using System.Threading.Tasks; // Needed for async/await (although not directly used in this snippet)

namespace MyDigitalKitchen.Models.ViewModels
{
    // This ViewModel prepares and holds recipe data for the RecipeList page.
    // It specifically handles grouping recipes.
    public class RecipeListViewModel
    {
        // This collection holds the recipes, grouped by the first letter of their title.
        // The UI (CollectionView) on RecipeList.xaml binds to this property.
        public ObservableCollection<RecipeGroup> GroupedRecipes { get; set; } = new ObservableCollection<RecipeGroup>();

        // Takes a list of recipes and organizes them into alphabetical groups.
        public void GroupRecipes(List<Recipe> recipesToGroup)
        {
            // Start with an empty list of groups.
            GroupedRecipes.Clear();

            // Only proceed if there are recipes to group.
            if (recipesToGroup != null && recipesToGroup.Any())
            {
                // Group the recipes:
                // 1. Order recipes by title first.
                // 2. Group them based on the first letter (A-Z, or # for others).
                // 3. Create RecipeGroup objects from these groups.
                // 4. Order the RecipeGroups alphabetically (# goes last).
                var grouped = recipesToGroup
                    .OrderBy(r => r.Title)
                    .GroupBy(r =>
                    {
                        // Determine the grouping key (first letter or '#').
                        if (string.IsNullOrEmpty(r.Title))
                        {
                            return "#";
                        }
                        char firstChar = r.Title[0];
                        if (char.IsLetter(firstChar))
                        {
                            return firstChar.ToString().ToUpper();
                        }
                        else // Handle digits and other symbols
                        {
                            return "#";
                        }
                    })
                    .Select(g => new RecipeGroup(g.Key, g.ToList()))
                    .OrderBy(g => g.Letter == "#" ? 27 : g.Letter[0]); // Custom sort to put # last

                // Add the created groups to the ObservableCollection that the UI is bound to.
                foreach (var group in grouped)
                {
                    GroupedRecipes.Add(group);
                }
            }
            // If the input list is empty or null, GroupedRecipes remains empty.
        }

        // The ViewModel does not fetch data itself; the page code-behind fetches
        // recipes from the repository and calls the GroupRecipes method.
    }
}