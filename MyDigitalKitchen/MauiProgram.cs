using Microsoft.Extensions.Logging;
using MyDigitalKitchen.Services;
using MyDigitalKitchen.Models.ViewModels;
using MyDigitalKitchen.Views; 

namespace MyDigitalKitchen
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

           
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<RecipeRepository>();

            
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddPage>();
            builder.Services.AddTransient<EditPage>();
            builder.Services.AddTransient<RecipeInfo>();
            builder.Services.AddTransient<RecipeList>();
            builder.Services.AddTransient<RecipeListViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            
            var dbService = app.Services.GetService<DatabaseService>();
            Task.Run(() => dbService.Init()).Wait(); 

            return app;
        }
    }
}