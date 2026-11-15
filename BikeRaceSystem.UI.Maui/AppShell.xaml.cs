using BikeRaceSystem.UI.Maui.Views.Pages;
using BikeRaceSystem.UI.Maui.Views.Pages.Race;

namespace BikeRaceSystem.UI.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("race", typeof(RacePage));
            Routing.RegisterRoute("statistics", typeof(StatisticsPage));
            Routing.RegisterRoute("settings", typeof(SettingsPage));
        }
    }
}
