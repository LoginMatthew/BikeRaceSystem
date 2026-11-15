using CommunityToolkit.Maui.Views;
using BikeRaceSystem.UI.Maui.Helpers;
using BikeRaceSystem.UI.Maui.ViewModels.Popups;
using BikeRaceSystem.Core.Interfaces.Localization;

namespace BikeRaceSystem.UI.Maui.Views.Popups;

public partial class ConfirmPopup : Popup
{
    public ConfirmPopup(string message)
    {
        InitializeComponent();

        var display = DeviceDisplay.MainDisplayInfo;
        Size = new Size(Math.Max(280, Math.Min(420, display.Width / display.Density - 32)), 250);

        var loc = ServiceHelper.GetService<ILocalizationService>();
        BindingContext = new ConfirmPopupViewModel(message, this, loc);
    }
}
