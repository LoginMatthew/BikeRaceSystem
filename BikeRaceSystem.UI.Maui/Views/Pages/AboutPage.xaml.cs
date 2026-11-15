using BikeRaceSystem.UI.Maui.ViewModels;

namespace BikeRaceSystem.UI.Maui.Views.Pages;

public partial class AboutPage : ContentPage
{
    public AboutPage(AboutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
