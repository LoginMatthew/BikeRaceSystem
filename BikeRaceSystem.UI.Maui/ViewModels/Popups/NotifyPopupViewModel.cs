using BikeRaceSystem.Core.Interfaces.Localization;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace BikeRaceSystem.UI.Maui.ViewModels.Popups
{
    public class NotifyPopupViewModel
    {
        public string Message { get; }
        public string OkLabel => _loc["OK"];

        private readonly Popup _popup;
        private readonly ILocalizationService _loc;

        public NotifyPopupViewModel(string message, Popup popup, ILocalizationService loc)
        {
            Message = message;
            _popup = popup;
            _loc = loc;
        }

        public IRelayCommand CloseCommand => new RelayCommand(() =>
        {
            _popup.Close();
        });

    }
}