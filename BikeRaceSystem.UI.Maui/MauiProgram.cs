using BikeRaceSystem.Core.Interfaces.Config;
using BikeRaceSystem.Core.Interfaces.Data;
using BikeRaceSystem.Core.Interfaces.Localization;
using BikeRaceSystem.Core.Interfaces.Race;
using BikeRaceSystem.Services.Config;
using BikeRaceSystem.Services.Data;
using BikeRaceSystem.Services.Factories;
using BikeRaceSystem.Services.Localization;
using BikeRaceSystem.Services.Race;
using BikeRaceSystem.UI.Maui.Helpers;
using BikeRaceSystem.UI.Maui.ViewModels;
using BikeRaceSystem.UI.Maui.ViewModels.Race;
using BikeRaceSystem.UI.Maui.Views.Extended.Race;
using BikeRaceSystem.UI.Maui.Views.Pages;
using BikeRaceSystem.UI.Maui.Views.Pages.Race;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace BikeRaceSystem.UI.Maui
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; }

        public static MauiApp CreateMauiApp()
        {
            var config = new ConfigService();
            var lang = config.Get("Language") ?? "hu";

            CultureInfo.CurrentUICulture = new CultureInfo(lang);
            CultureInfo.CurrentCulture = new CultureInfo(lang);

            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            //Services
            builder.Services.AddSingleton<ILocalizationService, LocalizationService>();

            builder.Services.AddSingleton<IAppConfigService, AppConfigService>();
            builder.Services.AddSingleton<IConfigService, ConfigService>();

            builder.Services.AddSingleton<RandomRaceService>();
            builder.Services.AddSingleton<SensorRaceService>();

            builder.Services.AddSingleton<IRaceServiceFactory, RaceServiceFactory>();
            builder.Services.AddSingleton<ISensorTransportFactory, SensorTransportFactory>();
            builder.Services.AddSingleton<IRaceEngineService, RaceEngineService>();
            builder.Services.AddSingleton<ICountdownService, CountdownService>();

            //Data
            builder.Services.AddSingleton<DbPathHolder>();

            builder.Services.AddSingleton<IDatabaseService>(sp =>
            {
                var holder = sp.GetRequiredService<DbPathHolder>();
                Console.WriteLine("DB PATH = " + holder.Path);
                return new DatabaseService(holder.Path);
            });

            builder.Services.AddTransient<IRaceResultRepository>(sp =>
            {
                var db = sp.GetRequiredService<IDatabaseService>();
                var holder = sp.GetRequiredService<DbPathHolder>();
                Console.WriteLine("DB PATH = " + holder.Path);

                return new RaceResultRepository(holder.Path, db);
            });



            //ViewModels
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddSingleton<RaceViewModel>();
            builder.Services.AddTransient<StatisticsViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();

            //Pages
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddSingleton<RacePage>();
            builder.Services.AddTransient<StatisticsPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<ExtendedStatisticsViewModel>();

            


#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            //DI
            Services = app.Services;

            return app;
        }
    }
}
