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

        }

        private void nextButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RecipeList());
        }
    }

}
