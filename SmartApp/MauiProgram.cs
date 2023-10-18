using Camera.MAUI;
using CommunityToolkit.Maui;
using DataAccess.Contexts;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartApp.Services;
using SmartApp.MVVM.ViewModels;
using SmartApp.MVVM.Views;
using SmartApp.Resources.Helpers;

namespace SmartApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCameraView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Rubik-Regular.ttf", "RubikRegular");
                    fonts.AddFont("fa-thin-100.ttf", "FontAwesomeThin");
                    fonts.AddFont("fa-light-300.ttf", "FontAwesomeLight");
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                });


            builder.Services.AddDbContext<SmartAppDbContext>(x => x.UseSqlite($"Data Source={DatabasePathFinder.GetPath()}", x => x.MigrationsAssembly(nameof(DataAccess))));
            builder.Services.AddSingleton<IotHubManager>();


            builder.Services.AddTransient<HttpClient>();
            builder.Services.AddSingleton<DateTimeService>();
            builder.Services.AddSingleton<WeatherService>();


            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<GetStartedViewModel>();
            builder.Services.AddSingleton<GetStartedPage>();

            builder.Services.AddSingleton<OverviewViewModel>();
            builder.Services.AddSingleton<OverviewPage>();




            return builder.Build();
        }
    }
}