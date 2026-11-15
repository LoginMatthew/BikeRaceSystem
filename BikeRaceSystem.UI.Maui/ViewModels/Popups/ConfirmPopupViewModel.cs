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
    private readonly Action _onConfirm;
    private readonly ILocalizationService _loc;

    public ConfirmPopupViewModel(string message, Popup popup, Action onConfirm, ILocalizationService loc)
    {
        Message = message;
        _popup = popup;
        _onConfirm = onConfirm;
        _loc = loc;
    }

    public IRelayCommand YesCommand => new RelayCommand(() =>
    {
        _popup.Close();
        _onConfirm?.Invoke();
    });

    public IRelayCommand NoCommand => new RelayCommand(() =>
    {
        _popup.Close();
    });
}
