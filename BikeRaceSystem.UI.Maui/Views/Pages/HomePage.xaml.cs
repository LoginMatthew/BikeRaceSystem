using BikeRaceSystem.UI.Maui.ViewModels;

namespace BikeRaceSystem.UI.Maui.Views.Pages;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
