using MyDigitalKitchen.Views;

namespace MyDigitalKitchen
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Search", "Search options will be added later", "OK");
        }

        private void nextButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RecipeList());
        }

        private void addButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPage());
        }
    }

}
