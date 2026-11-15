using BikeRaceSystem.Core.Interfaces.Localization;
using BikeRaceSystem.UI.Maui.ViewModels;

namespace BikeRaceSystem.UI.Maui.Views.Pages.Race;

public partial class RaceSettingsPage : ContentPage
{
    public RaceSettingsPage(RaceViewModel raceVm, ILocalizationService loc)
    {
        InitializeComponent();
        BindingContext = new RaceSettingsViewModel(raceVm, loc);
    }
}