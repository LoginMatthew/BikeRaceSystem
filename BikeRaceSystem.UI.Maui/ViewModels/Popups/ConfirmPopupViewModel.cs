using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using BikeRaceSystem.Core.Interfaces.Localization;

namespace BikeRaceSystem.UI.Maui.ViewModels.Popups;

public class ConfirmPopupViewModel
{
    public string Message { get; }
    public string YesLabel => _loc["Yes"];
    public string NoLabel => _loc["No"];

    private readonly Popup _popup;
    private readonly ILocalizationService _loc;

    public ConfirmPopupViewModel(string message, Popup popup, ILocalizationService loc)
    {
        Message = message;
        _popup = popup;
        _loc = loc;
    }

    public IAsyncRelayCommand YesCommand => new AsyncRelayCommand(async () =>
    {
        await _popup.CloseAsync(true);
    });

    public IAsyncRelayCommand NoCommand => new AsyncRelayCommand(async () =>
    {
        await _popup.CloseAsync(false);
    });
}
