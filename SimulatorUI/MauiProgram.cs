using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimulatorEngine;
using SimulatorUI.Components;
using SimulatorUI.Definitions;
using SimulatorUI.Sharing;
using SimulatorUI.Sharing.Cloud;
using SimulatorUI.Sharing.File;
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
            builder.Services.AddSingleton<IParticlesManager, ParticlesManager>();

            var sharingMethod = Enum.TryParse(config.GetSection("SharingMethod").Value, out SharingMethod method)
                ? method
                : SharingMethod.None;

            switch (sharingMethod)
            {
                case SharingMethod.Cloud:
                    builder.Services.AddSingleton<IShareManager, CloudShareManager>();
                    break;
                case SharingMethod.File:
                    builder.Services.AddSingleton<IShareManager, FileShareManager>();
                    break;
            }

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
