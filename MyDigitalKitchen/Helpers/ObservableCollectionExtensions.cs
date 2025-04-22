using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDigitalKitchen.Helpers
{
    // This class provides helpful extension methods for collections.
    public static class ObservableCollectionExtensions
    {
        // Converts a standard collection (like List<T>) into an ObservableCollection<T>.
        // ObservableCollection is useful for UI binding that needs to react to changes.
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            // Create the new collection.
            var observableCollection = new ObservableCollection<T>();

            // Copy items from the source collection.
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    observableCollection.Add(item);
                }
            }

            // Return the new collection.
            return observableCollection;
        }
    }
}