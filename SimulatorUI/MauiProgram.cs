using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimulatorEngine;
using SimulatorUI.Api;
using SimulatorUI.Components;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace SimulatorUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.base.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IConfiguration>(config);
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<UploadPage>();
            builder.Services.AddTransient<DownloadPage>();
            builder.Services.AddSingleton<IApiManager, ApiManager>();
            builder.Services.AddSingleton<IParticlesManager, ParticlesManager>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
