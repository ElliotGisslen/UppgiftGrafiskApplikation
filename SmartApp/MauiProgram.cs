using CommunityToolkit.Maui;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<SmartAppDbContext>(x => x.UseSqlite($"Data Source={DatabasePathFinder.GetPath()}", x => x.MigrationsAssembly(nameof(DataAccess))));


            return builder.Build();
        }
    }
}