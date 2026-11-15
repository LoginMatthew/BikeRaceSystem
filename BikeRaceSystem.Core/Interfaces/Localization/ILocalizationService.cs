namespace BikeRaceSystem.Core.Interfaces.Localization
{
    public interface ILocalizationService
    {
        string this[string key] { get; }

        event Action? LanguageChanged;
        void ChangeLanguage(string lang);
    }
}
