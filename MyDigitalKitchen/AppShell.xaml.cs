using MyDigitalKitchen.Views; 

namespace MyDigitalKitchen
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            
            Routing.RegisterRoute(nameof(AddPage), typeof(AddPage));
            Routing.RegisterRoute(nameof(RecipeList), typeof(RecipeList));
            Routing.RegisterRoute(nameof(RecipeInfo), typeof(RecipeInfo));
            Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

            
        }
    }
}
