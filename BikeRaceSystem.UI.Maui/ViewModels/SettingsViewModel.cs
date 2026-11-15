using BikeRaceSystem.Core.Interfaces.Config;
using BikeRaceSystem.Core.Interfaces.Data;
using BikeRaceSystem.Core.Interfaces.Localization;
using BikeRaceSystem.UI.Maui.Helpers;
using BikeRaceSystem.UI.Maui.Views.Popups;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BikeRaceSystem.UI.Maui.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {

        private readonly IConfigService _config;
        private readonly IAppConfigService _appConfigService;
        private readonly ILocalizationService _loc;
        private readonly IDatabaseService _databaseService;

        public SettingsViewModel(IConfigService config, IAppConfigService appConfigService, ILocalizationService loc, IDatabaseService databaseService)
        {
            _config = config;
            _appConfigService = appConfigService;
            _loc = loc;
            _databaseService = databaseService;

            // Language and RaceMode only from IConfigService
            SelectedRaceMode = _config.Get("RaceMode") ?? "Random";
            SelectedLanguage = _config.Get("Language") ?? "hu";

            // Sensor settings from AppConfigService
            var appConfig = _appConfigService.Load();
            Sensor1Transport = appConfig.Sensor.Racer1.Transport;
            Sensor1Ip = appConfig.Sensor.Racer1.Ip;
            Sensor1Port = appConfig.Sensor.Racer1.TcpPort;
            Sensor1ComPort = appConfig.Sensor.Racer1.ComPort;
            Sensor1BaudRate = appConfig.Sensor.Racer1.BaudRate;
            Sensor2Transport = appConfig.Sensor.Racer2.Transport;
            Sensor2Ip = appConfig.Sensor.Racer2.Ip;
            Sensor2Port = appConfig.Sensor.Racer2.TcpPort;
            Sensor2ComPort = appConfig.Sensor.Racer2.ComPort;
            Sensor2BaudRate = appConfig.Sensor.Racer2.BaudRate;

            _loc.LanguageChanged += () =>
            {
                OnPropertyChanged(nameof(BackLabel));
                OnPropertyChanged(nameof(SettingLabel));
                OnPropertyChanged(nameof(SaveLabel));
                OnPropertyChanged(nameof(SavedMessage));
                OnPropertyChanged(nameof(DataSourceLabel));
                OnPropertyChanged(nameof(SensorSettingsLabel));
                OnPropertyChanged(nameof(LanguageLabel));
                OnPropertyChanged(nameof(IPplaceholder));
                OnPropertyChanged(nameof(ResetDatabaseLabel));
                OnPropertyChanged(nameof(ConfirmResetDatabase));
                OnPropertyChanged(nameof(DatabaseResetSuccess));
                OnPropertyChanged(nameof(Sensor1Label));
                OnPropertyChanged(nameof(Sensor2Label));
                OnPropertyChanged(nameof(TransportLabel));
                OnPropertyChanged(nameof(ComPortLabel));
                OnPropertyChanged(nameof(BaudRateLabel));
                OnPropertyChanged(nameof(SensorProtocolHelp));
            };
        }

        public List<string> RaceModes { get; } = new() { "Random", "Sensor" };
        public List<string> Languages { get; } = new() { "hu", "en", "de", "fr", "ja" };
        public List<string> SensorTransports { get; } = new() { "TCP", "COM", "Bluetooth SPP" };

        public string BackLabel => _loc["Back"];
        public string SettingLabel => _loc["Settings"];
        public string SaveLabel => _loc["Save"];
        public string SavedMessage => _loc["Saved"];
        public string LanguageLabel => _loc["Language"];
        public string DataSourceLabel => _loc["DataSource"];
        public string SensorSettingsLabel => _loc["SensorSettings"];
        public string IPplaceholder => _loc["IPAddress"];
        public string ResetDatabaseLabel => _loc["ResetDatabase"];
        public string ConfirmResetDatabase => _loc["ConfirmResetDatabase"];
        public string DatabaseResetSuccess => _loc["DatabaseResetSuccess"];
        public string Sensor1Label => _loc["Sensor1"];
        public string Sensor2Label => _loc["Sensor2"];
        public string TransportLabel => _loc["Transport"];
        public string ComPortLabel => _loc["ComPort"];
        public string BaudRateLabel => _loc["BaudRate"];
        public string SensorProtocolHelp => _loc["SensorProtocolHelp"];

        [ObservableProperty]
        private string selectedRaceMode;

        partial void OnSelectedRaceModeChanged(string value)
        {
            _config.Set("RaceMode", value);
        }

        [ObservableProperty]
        private string selectedLanguage;

        partial void OnSelectedLanguageChanged(string value)
        {
            _config.Set("Language", value);
            _loc.ChangeLanguage(value);
        }

        [ObservableProperty]
        private string sensor1Transport;

        [ObservableProperty]
        private string sensor1Ip;

        [ObservableProperty] private int sensor1Port;
        [ObservableProperty] private string sensor1ComPort;
        [ObservableProperty] private int sensor1BaudRate;
        [ObservableProperty] private string sensor2Transport;
        [ObservableProperty] private string sensor2Ip;
        [ObservableProperty] private int sensor2Port;
        [ObservableProperty] private string sensor2ComPort;
        [ObservableProperty] private int sensor2BaudRate;

        [RelayCommand]
        private async Task SaveSensorSettings()
        {
            var confirmationResult = await Shell.Current.ShowPopupAsync(new ConfirmPopup(_loc["ConfirmSave"]));
            if (confirmationResult is not true)
                return;

            _config.Set("RaceMode", SelectedRaceMode);
            _config.Set("Language", SelectedLanguage);

            var cfg = _appConfigService.Load();
            cfg.Sensor.Racer1.Transport = Sensor1Transport;
            cfg.Sensor.Racer1.Ip = Sensor1Ip;
            cfg.Sensor.Racer1.TcpPort = Sensor1Port;
            cfg.Sensor.Racer1.ComPort = Sensor1ComPort;
            cfg.Sensor.Racer1.BaudRate = Sensor1BaudRate;
            cfg.Sensor.Ip = Sensor1Ip;
            cfg.Sensor.Port = Sensor1Port;
            cfg.Sensor.Racer2.Transport = Sensor2Transport;
            cfg.Sensor.Racer2.Ip = Sensor2Ip;
            cfg.Sensor.Racer2.TcpPort = Sensor2Port;
            cfg.Sensor.Racer2.ComPort = Sensor2ComPort;
            cfg.Sensor.Racer2.BaudRate = Sensor2BaudRate;
            _appConfigService.Save(cfg);

            _loc.ChangeLanguage(SelectedLanguage);
            await Shell.Current.ShowPopupAsync(new NotifyPopup(_loc["Saved"]));
        }

        [RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("///home");
        }

        [RelayCommand]
        private async Task ResetDatabase()
        {
            var confirmationResult = await Shell.Current.ShowPopupAsync(new ConfirmPopup(_loc["ConfirmResetDatabase"]));
            if (confirmationResult is not true)
                return;

            var newPath = await _databaseService.ResetDatabaseAsync();
            var holder = MauiProgram.Services.GetRequiredService<DbPathHolder>();
            holder.Path = newPath;

            await Shell.Current.ShowPopupAsync(new NotifyPopup(_loc["DatabaseResetSuccess"]));
        }
    }
}
