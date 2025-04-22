using System;
using System.Collections.Generic;
using System.Linq; // Needed for LINQ operations (though not directly in this snippet, good practice)
using SQLite; // Provides attributes and functionality for SQLite mapping
using SQLiteNetExtensions.Attributes; // Provides additional attributes like [Ignore]

namespace MyDigitalKitchen.Models
{
    // This class represents a single Recipe and is mapped to a table in the SQLite database.
    [Table("Recipes")] // Specifies the name of the SQLite table for this class.
    public class Recipe
    {
        // --- Database Mapping Properties ---

        // The Id property serves as the primary key for the Recipes table.
        // [PrimaryKey] marks it as the primary key.
        // [AutoIncrement] tells SQLite to automatically generate a unique ID when a new row is inserted.
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // These properties map directly to columns in the Recipes table by default
        // (unless specified otherwise with [Column] or ignored).
        public string Title { get; set; }
        public string MealType { get; set; }

        // The Ingredients list is complex and cannot be stored directly by SQLite-Net-PCL.
        // [Ignore] tells SQLite-Net-PCL to skip this property when mapping to the database.
        // We handle the storage of this list manually via the IngredientsBlob property.
        [Ignore]
        public List<string> Ingredients { get; set; } = new List<string>();

        // This property is used to store the Ingredients list in a serialized format (like a string).
        // We manually convert the List<string> to/from this string property in the RecipeRepository.
        public string IngredientsBlob { get; set; }

        // More properties mapping to database columns.
        public string Time { get; set; } // e.g., "30 minutes"
        public string Directions { get; set; } // Instructions for the recipe
        public string ImagePath { get; set; } // Path to an image for the recipe (assuming string path)
        public string Notes { get; set; } // Any additional notes about the recipe

        // Boolean flag to indicate if the recipe is a favorite.
        public bool IsFavorite { get; set; }

        // Stores the last time the recipe was accessed. Useful for sorting recents.
        // DateTime is typically stored as a number (ticks) in SQLite by default.
        public DateTime LastAccessed { get; set; }

        // Note: Manual serialization/deserialization logic for Ingredients <-> IngredientsBlob
        // is implemented in the RecipeRepository class.
    }
}