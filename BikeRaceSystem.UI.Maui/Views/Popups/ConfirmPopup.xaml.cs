using CommunityToolkit.Maui.Views;
using BikeRaceSystem.UI.Maui.Helpers;
using BikeRaceSystem.UI.Maui.ViewModels.Popups;
using BikeRaceSystem.Core.Interfaces.Localization;

namespace BikeRaceSystem.UI.Maui.Views.Popups;

public partial class ConfirmPopup : Popup
{
    public ConfirmPopup(string message, Action onConfirm)
    {
        InitializeComponent();

        var loc = ServiceHelper.GetService<ILocalizationService>();
        BindingContext = new ConfirmPopupViewModel(message, this, onConfirm, loc);
    }
}
