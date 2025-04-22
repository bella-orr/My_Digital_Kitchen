using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Attributes; 

namespace MyDigitalKitchen.Models
{
    [Table("Recipes")] 
    public class Recipe 
    {
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }

        public string Title { get; set; }
        public string MealType { get; set; }

     
        [Ignore]
        public List<string> Ingredients { get; set; } = new List<string>();

       
        public string IngredientsBlob { get; set; }


        public string Time { get; set; } 
        public string Directions { get; set; }
        public string ImagePath { get; set; } 
        public string Notes { get; set; }

        public bool IsFavorite { get; set; }

        public DateTime LastAccessed { get; set; } 

      
    }
}