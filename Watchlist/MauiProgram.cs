using Microsoft.Extensions.Logging;
using Watchlist.Message;
using Watchlist.ViewModel;
using Watchlist.Service;
using Watchlist.Pages;

namespace Watchlist
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

            // Register services
            builder.Services.AddSingleton<IMessageDialogService, MauiMessageDialogService>();
            builder.Services.AddSingleton<IMovieService, MovieService>();

            // Register ViewModels
            builder.Services.AddSingleton<MoviesViewModel>();
            builder.Services.AddTransient<MovieDataViewModel>();

            // Register Pages
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<MovieDataPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
