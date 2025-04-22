using MyDigitalKitchen.Models;
using System.Collections.ObjectModel;
using System.Linq;
using MyDigitalKitchen.Helpers;
using System.Threading.Tasks;

namespace MyDigitalKitchen.Models.ViewModels
{
    public class RecipeListViewModel
    {
       
        public ObservableCollection<RecipeGroup> GroupedRecipes { get; set; } = new ObservableCollection<RecipeGroup>();

       
        
        public void GroupRecipes(List<Recipe> recipesToGroup)
        {
            GroupedRecipes.Clear(); // Clear existing groups

            if (recipesToGroup != null && recipesToGroup.Any())
            {
                var grouped = recipesToGroup
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