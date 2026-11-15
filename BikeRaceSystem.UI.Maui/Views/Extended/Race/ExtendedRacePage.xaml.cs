using BikeRaceSystem.UI.Maui.ViewModels;

namespace BikeRaceSystem.UI.Maui.Views.Extended.Race;
public partial class ExtendedRacePage : ContentPage
{
    public ExtendedRacePage(ExtendedRaceViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
