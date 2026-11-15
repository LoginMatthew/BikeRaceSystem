using BikeRaceSystem.Core.Interfaces.Localization;
using BikeRaceSystem.UI.Maui.Helpers;
using BikeRaceSystem.UI.Maui.ViewModels.Popups;
using CommunityToolkit.Maui.Views;

namespace BikeRaceSystem.UI.Maui.Views.Popups;

public partial class NotifyPopup : Popup
{
    public NotifyPopup(string message)
    {
        InitializeComponent();

        var display = DeviceDisplay.MainDisplayInfo;
        Size = new Size(Math.Max(280, Math.Min(400, display.Width / display.Density - 32)), 220);

        var loc = ServiceHelper.GetService<ILocalizationService>();
        BindingContext = new NotifyPopupViewModel(message, this, loc);
    }
}
