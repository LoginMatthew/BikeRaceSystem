using BikeRaceSystem.Core.Interfaces.Data;
using BikeRaceSystem.Core.Interfaces.Localization;
using BikeRaceSystem.Core.Interfaces.Race;
using BikeRaceSystem.Core.Models;
using BikeRaceSystem.UI.Maui.ViewModels.Race;
using BikeRaceSystem.UI.Maui.Views.Extended.Race;
using BikeRaceSystem.UI.Maui.Views.Extended.Statistics;
using BikeRaceSystem.UI.Maui.Views.Pages.Race;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace BikeRaceSystem.UI.Maui.ViewModels
{
    public partial class RaceViewModel : ObservableObject
    {
        private readonly ILocalizationService _loc;
        private readonly IServiceProvider _services;
        private readonly ICountdownService _countdown;
        private readonly ILogger<RaceViewModel> _logger;
        
        private readonly IRaceServiceFactory _factory;
        private readonly IRaceService _raceService;
        private readonly IRaceEngineService _engineService;

        private CancellationTokenSource? _countdownCts;
        private Window? _extendedWindow;
        private Page? _extendedContentPage;

        public RaceViewModel(ILocalizationService loc, IServiceProvider services, IRaceServiceFactory factory, IRaceEngineService raceEngineService, ICountdownService countdown, ILogger<RaceViewModel> logger)
        {
            _loc = loc;
            _services = services;
            _factory = factory;
            _engineService = raceEngineService;
            _raceService = _factory.GetService();
            _engineService.ProgressChanged += OnProgressChanged;
            _engineService.Failed += OnRaceFailed;
            _loc.LanguageChanged += OnLanguageChanged;
            _countdown = countdown;
            _logger = logger;

            _countdown.Tick += OnCountdownTick;
            _countdown.Finished += OnCountdownFinished;

        }

        //Extended window
        [ObservableProperty]
        private string? currentExtendedMode; // "Race", "Statistics", null

        // Racer names
        [ObservableProperty] private string racer1Name = "Versenyző 1";
        [ObservableProperty] private string racer2Name = "Versenyző 2";

        // App title
        [ObservableProperty] private string appTitle = "Iskola versenye";

        // Race parameters
        [ObservableProperty] private int distance = 500;
        [ObservableProperty] private int time = 30;
        [ObservableProperty] private int countdownTime = 10;
        [ObservableProperty] private int countdownValue;
        [ObservableProperty] private bool isCountdownVisible;
        [ObservableProperty] private bool isRaceRunning;
        [ObservableProperty] private bool isStartVisible;
        [ObservableProperty] private string startSignalText;

        // Speeds
        [ObservableProperty] private double racer1Speed;
        [ObservableProperty] private double racer2Speed;

        // Distances
        [ObservableProperty] private double racer1Distance;
        [ObservableProperty] private double racer2Distance;

        [ObservableProperty] private bool isExtendedWindowOpen;

        [ObservableProperty] private string racer1Status;
        [ObservableProperty] private string racer2Status;

        [ObservableProperty] private int racer1Place;
        [ObservableProperty] private int racer2Place;

        [ObservableProperty] private bool raceFinished;

        [ObservableProperty] private string racer1Code;
        [ObservableProperty] private string racer2Code;
        [ObservableProperty] private double racer1Time;
        [ObservableProperty] private double racer2Time;

        public string ExtendButtonLabel => IsExtendedWindowOpen ? _loc["ExtendClose"] : _loc["ExtendOpen"];

        [ObservableProperty]
        private string backgroundImagePath = "Resources/Images/race_background.jpg";

        public double Racer1Progress => Distance == 0 ? 0 : Racer1Distance / Distance;
        public double Racer2Progress => Distance == 0 ? 0 : Racer2Distance / Distance;

        public double MaxSpeed { get; set; } = 60;

        // Localization
        public string StartLabel => _loc["Start"];
        public string StopLabel => _loc["Stop"];
        public string ResetLabel => _loc["Reset"];
        public string EditParametersLabel => _loc["EditParameters"];
        public string ExtendRaceLabel => _loc["ExtendRace"];
        public string ExtendStatisticsLabel => _loc["ExtendStatistics"];
        public string CloseExtendedLabel => _loc["CloseExtended"];
        public string DistanceLabel => _loc["Distance"];
        public string TimeLabel => _loc["Time"];
        public string BackLabel => _loc["Back"];
        public string StatisticsLabel => _loc["Statistics"];
        public string StartSignalLabel => _loc["StartSignal"];
        public string CountdownLabel => _loc["CountdownLabel"];

        [RelayCommand]
        private async Task StartRace()
        {
            ResetStatus();

            IsRaceRunning = true;
            IsCountdownVisible = true;
            CountdownValue = CountdownTime;

            _countdownCts?.Cancel();
            _countdownCts = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                await _countdown.StartAsync(CountdownTime, _countdownCts.Token);
            });
        }

        [RelayCommand]
        private void StopRace()
        {
            _engineService.Stop();

            _countdownCts?.Cancel();
            IsCountdownVisible = false;
            IsStartVisible = false;

            IsRaceRunning = false;
        }

        [RelayCommand]
        private void ResetRace()
        {
            _engineService.Stop();

            _countdownCts?.Cancel();
            IsCountdownVisible = false;
            IsStartVisible = false;

            ResetStatus();
            IsRaceRunning = false;
        }


        [RelayCommand]
        private async Task EditParameters()
        {
            var page = new RaceSettingsPage(this, _loc);
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        private async Task NavigateStatistics()
        {
            await Shell.Current.GoToAsync("///statistics");
        }

        [RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("///home");
        }
        [RelayCommand]
        private void ShowExtendedRace()
        {
            if (IsRaceRunning)
                return;

            if (CurrentExtendedMode == "Race")
                return;

            var raceVm = _services.GetRequiredService<RaceViewModel>();
            var extendedVm = new ExtendedRaceViewModel(raceVm);
            var page = new ExtendedRacePage(extendedVm);

            CurrentExtendedMode = "Race";
            SetExtendedContent(page);
        }

        [RelayCommand]
        private void ShowExtendedStatistics()
        {
            if (IsRaceRunning)
                return;

            if (CurrentExtendedMode == "Statistics")
                return;

            var vm = _services.GetRequiredService<ExtendedStatisticsViewModel>();
            var page = new ExtendedStatisticsPage(vm);

            CurrentExtendedMode = "Statistics";
            SetExtendedContent(page);
        }

        [RelayCommand]
        private void CloseExtendedWindow()
        {
            if (IsRaceRunning)
                return;

            if (_extendedWindow != null)
            {
                Application.Current.CloseWindow(_extendedWindow);
                _extendedWindow = null;
                _extendedContentPage = null;
                CurrentExtendedMode = null;
                IsExtendedWindowOpen = false;
            }
        }

        private void ResetStatus()
        {
            Racer1Distance = 0;
            Racer2Distance = 0;
            Racer1Speed = 0;
            Racer2Speed = 0;
            Racer1Status = "";
            Racer2Status = "";
            Racer1Place = 0;
            Racer2Place = 0;
            RaceFinished = false;

            OnPropertyChanged(nameof(Racer1Progress));
            OnPropertyChanged(nameof(Racer2Progress));
        }

        private void OnCountdownTick(int value)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CountdownValue = value;
            });
        }

        private void OnCountdownFinished()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (!IsRaceRunning)
                    return;

                IsCountdownVisible = false;
                StartSignalText = StartSignalLabel;
                IsStartVisible = true;

                await Task.Delay(1000);

                IsStartVisible = false;

                _engineService.Start(_raceService, Distance);
            });
        }


        partial void OnIsExtendedWindowOpenChanged(bool value)
        {
            OnPropertyChanged(nameof(ExtendButtonLabel));
        }

        partial void OnRaceFinishedChanged(bool value)
        {
            if (value)
            {
                IsRaceRunning = false;
                _ = SaveRaceResultSafelyAsync();
            }
        }

        private void SetExtendedContent(Page page)
        {
            if (_extendedWindow == null)
            {
                _extendedWindow = new Window(page);

                _extendedWindow.Destroying += (s, e) =>
                {
                    IsExtendedWindowOpen = false;
                    _extendedWindow = null;
                    _extendedContentPage = null;
                    CurrentExtendedMode = null;
                };

                Application.Current.OpenWindow(_extendedWindow);
                IsExtendedWindowOpen = true;
                _extendedContentPage = page;
                return;
            }

            _extendedContentPage = page;
            _extendedWindow.Page = page;
        }

        private void OnLanguageChanged()
        {
            OnPropertyChanged(nameof(StartLabel));
            OnPropertyChanged(nameof(StopLabel));
            OnPropertyChanged(nameof(ResetLabel));
            OnPropertyChanged(nameof(EditParametersLabel));
            OnPropertyChanged(nameof(ExtendRaceLabel));
            OnPropertyChanged(nameof(ExtendStatisticsLabel));
            OnPropertyChanged(nameof(CloseExtendedLabel));
            OnPropertyChanged(nameof(DistanceLabel));
            OnPropertyChanged(nameof(TimeLabel));
            OnPropertyChanged(nameof(BackLabel));
            OnPropertyChanged(nameof(StatisticsLabel));
            OnPropertyChanged(nameof(ExtendButtonLabel));            
            OnPropertyChanged(nameof(StartSignalLabel));
            OnPropertyChanged(nameof(CountdownLabel));
        }

        private void OnProgressChanged(double d1, double d2, double s1, double s2, double elapsed, bool r1Finished, bool r2Finished)
        {
            MainThread.BeginInvokeOnMainThread(() => ApplyProgress(d1, d2, s1, s2, elapsed, r1Finished, r2Finished));
        }

        private void ApplyProgress(double d1, double d2, double s1, double s2, double elapsed, bool r1Finished, bool r2Finished)
        {
            Racer1Distance = d1;
            Racer2Distance = d2;

            Racer1Speed = s1;
            Racer2Speed = s2;

            if (r1Finished && Racer1Place == 0)
            {
                Racer1Place = 1;
                Racer1Time = elapsed;
                Racer1Status = $"🏆 {Racer1Name} {_loc["Winner"]}! {_loc["Time"]}: {elapsed:0.0} {_loc["Seconds"]}";
            }

            if (r2Finished && Racer2Place == 0)
            {
                Racer2Place = r1Finished ? 2 : 1;
                Racer2Status = $"🏆 {Racer2Name} {_loc["Finished"]}! {_loc["Time"]}: {elapsed:0.0} {_loc["Seconds"]}";
                Racer2Time = elapsed;
            }

            OnPropertyChanged(nameof(Racer1Progress));
            OnPropertyChanged(nameof(Racer2Progress));

            if (r1Finished && r2Finished)
            {
                RaceFinished = true;
            }
        }

        private void OnRaceFailed(Exception exception)
        {
            _logger.LogError(exception, "The race data source failed.");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                IsRaceRunning = false;
                IsCountdownVisible = false;
                IsStartVisible = false;
                if (Shell.Current is not null)
                    await Shell.Current.DisplayAlert(_loc["Error"], _loc["SensorConnectionFailed"], _loc["OK"]);
            });
        }

        private async Task SaveRaceResultSafelyAsync()
        {
            try
            {
                var repo = _services.GetRequiredService<IRaceResultRepository>();
                var race = new RaceResultModel
                {
                    Date = DateTime.Now,
                    RaceName = "Iskolai futam",
                    Distance = Distance,
                    Participants = new List<RaceParticipantResultModel>
                    {
                        new RaceParticipantResultModel { Name = Racer1Name, UniqueCode = Racer1Code, Time = Racer1Time, Place = Racer1Place },
                        new RaceParticipantResultModel { Name = Racer2Name, UniqueCode = Racer2Code, Time = Racer2Time, Place = Racer2Place }
                    }
                };

                await repo.SaveRaceAsync(race);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save race results.");
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    if (Shell.Current is not null)
                        await Shell.Current.DisplayAlert(_loc["Error"], _loc["SaveFailed"], _loc["OK"]);
                });
            }
        }

    }
}
