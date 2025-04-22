using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDigitalKitchen.Helpers
{

    public static class ObservableCollectionExtensions
    {


        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
          
            var observableCollection = new ObservableCollection<T>();

           
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    observableCollection.Add(item);
                }
            }

           
            return observableCollection;
        }
    }
}