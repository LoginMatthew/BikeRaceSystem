using BikeRaceSystem.Core.Interfaces.Config;
using BikeRaceSystem.Core.Interfaces.Localization;
using BikeRaceSystem.Localization;
using System.Globalization;


namespace BikeRaceSystem.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public event Action? LanguageChanged;

        public LocalizationService(IConfigService config)
        {
            var lang = config.Get("Language") ?? "hu";
            SetCulture(lang);
        }

        public void ChangeLanguage(string lang)
        {
            SetCulture(lang);
            LanguageChanged?.Invoke();
        }

        private void SetCulture(string lang)
        {
            var culture = new CultureInfo(lang);

            CultureInfo.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            AppResources.Culture = culture;
        }

        public string this[string key]
        {
            get
            {
                var value = AppResources.ResourceManager.GetString(key, AppResources.Culture);
                return value ?? key;
            }
        }
    }

}

