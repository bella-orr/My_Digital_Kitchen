using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDigitalKitchen
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MealType { get; set; } 
        public List<string> Ingredients { get; set; } = new List<string>();
        public string Time { get; set; } 
        public string Directions { get; set; }
        public string ImagePath { get; set; } 
        public string Notes { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime LastAccessed { get; set; }


        //this represents the recipe
        public string Name { get; set; }
    }
}
