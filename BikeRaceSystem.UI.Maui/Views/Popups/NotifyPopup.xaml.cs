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

        var loc = ServiceHelper.GetService<ILocalizationService>();
        BindingContext = new NotifyPopupViewModel(message, this, loc);
    }
}
