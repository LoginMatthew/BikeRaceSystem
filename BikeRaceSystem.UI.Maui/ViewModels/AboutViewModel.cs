using BikeRaceSystem.Core.Interfaces.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BikeRaceSystem.UI.Maui.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    private readonly ILocalizationService _loc;

    public string PageTitle => _loc["AboutTitle"];
    public string Description => _loc["AboutDescription"];
    public string DeveloperCredit => _loc["AboutDeveloper"];
    public string BackLabel => _loc["Back"];

    public AboutViewModel(ILocalizationService loc)
    {
        _loc = loc;
        _loc.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged()
    {
        OnPropertyChanged(nameof(PageTitle));
        OnPropertyChanged(nameof(Description));
        OnPropertyChanged(nameof(DeveloperCredit));
        OnPropertyChanged(nameof(BackLabel));
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
