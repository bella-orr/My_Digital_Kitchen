using MyDigitalKitchen.Views; // Needed to reference your page types

namespace MyDigitalKitchen
{
    // This class is the code-behind for the application's Shell.
    // The Shell is used for navigation within the app.
    public partial class AppShell : Shell
    {
        // Constructor for the AppShell.
        public AppShell()
        {
            InitializeComponent(); // Initialize the Shell UI from AppShell.xaml

            // Register routes for pages. This allows navigating to pages
            // using a string name (the route) with Shell.Current.GoToAsync().
            // It also enables passing parameters via the route URL.
            Routing.RegisterRoute(nameof(AddPage), typeof(AddPage));
            Routing.RegisterRoute(nameof(RecipeList), typeof(RecipeList));
            Routing.RegisterRoute(nameof(RecipeInfo), typeof(RecipeInfo));
            Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

            // Any other pages you navigate to using GoToAsync should also be registered here.
        }
    }
}