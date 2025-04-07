using MyDigitalKitchen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDigitalKitchen.Models.ViewModels
{
    public class RecipeListViewModel
    {
        public ObservableCollection<RecipeGroup> GroupedRecipes { get; set; }

        public RecipeListViewModel()
        {

            //for testing
            GroupedRecipes = new ObservableCollection<RecipeGroup>
            {
                new RecipeGroup("A", new List<Recipe>
                {
                    new Recipe { Name = "Apple Pie" },
                    new Recipe { Name = "Avocado Toast" }
                }),
                new RecipeGroup("B", new List<Recipe>
                {
                    new Recipe { Name = "Banana Bread" },
                    new Recipe { Name = "Beef Stew" }
                }),
                new RecipeGroup("C", new List<Recipe>
                {
                    new Recipe { Name = "Chocolate Cake" },
                    new Recipe { Name = "Chicken Parmesan" }
                })
            };

        }

    }
}
