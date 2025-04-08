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
        private List<Recipe> _allRecipes = new();

        private string _selectedMealType = "All";
        private string _selectedDateSort = "Newest First";
        public RecipeListViewModel()
        {

            //for testing
            GroupedRecipes = new ObservableCollection<RecipeGroup>
            {
                new RecipeGroup("A", new List<Recipe>
                {
                    new Recipe { Title = "Apple Pie" },
                    new Recipe { Title = "Avocado Toast" }
                }),
                new RecipeGroup("B", new List<Recipe>
                {
                    new Recipe { Title = "Banana Bread" },
                    new Recipe { Title = "Beef Stew" }
                }),
                new RecipeGroup("C", new List<Recipe>
                {
                    new Recipe { Title = "Chocolate Cake" },
                    new Recipe { Title = "Chicken Parmesan" }
                })
            };

        }

    }
}
