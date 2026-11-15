using BikeRaceSystem.UI.Maui.ViewModels;

namespace BikeRaceSystem.UI.Maui.Views.Pages.Race;

public partial class RacePage : ContentPage
{
    public RacePage(RaceViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
