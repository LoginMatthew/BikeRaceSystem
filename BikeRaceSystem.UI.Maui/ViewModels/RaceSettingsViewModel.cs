using BikeRaceSystem.Core.Interfaces.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BikeRaceSystem.UI.Maui.ViewModels
{
    public partial class RaceSettingsViewModel : ObservableObject
    {
        private readonly RaceViewModel _raceVm;
        private readonly ILocalizationService _loc;

        [ObservableProperty] private string racer1Name;
        [ObservableProperty] private string racer2Name;
        [ObservableProperty] private int distance;
        [ObservableProperty] private int time;
        [ObservableProperty] private string appTitle;
        [ObservableProperty] private int countdownTime;
        [ObservableProperty] private string selectedImagePath;

        public string AppTitleLabel => _loc["AppTitle"];
        public string Title => _loc["RaceSettingsTitle"];
        public string Racer1Label => _loc["Racer1"];
        public string Racer2Label => _loc["Racer2"];
        public string DistanceLabel => _loc["Distance"];
        public string TimeLabel => _loc["Time"];
        public string SaveLabel => _loc["Save"];
        public string CancelLabel => _loc["Cancel"];
        public string CountdownLabel => _loc["CountdownLabel"];
        public string BrowseImageLabel => _loc["BrowseImage"];

        public RaceSettingsViewModel(RaceViewModel raceVm, ILocalizationService loc)
        {
            _raceVm = raceVm;
            _loc = loc;

            Racer1Name = _raceVm.Racer1Name;
            Racer2Name = _raceVm.Racer2Name;
            Distance = _raceVm.Distance;
            Time = _raceVm.Time;
            CountdownTime = _raceVm.CountdownTime;
            AppTitle = _raceVm.AppTitle;
        }

        [RelayCommand]
        private async Task Save()
        {
            _raceVm.Racer1Name = Racer1Name;
            _raceVm.Racer2Name = Racer2Name;
            _raceVm.Distance = Distance;
            _raceVm.Time = Time;
            _raceVm.CountdownTime = CountdownTime;

            if (!string.IsNullOrWhiteSpace(AppTitle))
                _raceVm.AppTitle = AppTitle;

            if (!string.IsNullOrWhiteSpace(SelectedImagePath))
                _raceVm.BackgroundImagePath = SelectedImagePath;

            await Shell.Current.Navigation.PopModalAsync();
        }

        [RelayCommand]
        private async Task BrowseImage()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = _loc["BrowseImage"],
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                SelectedImagePath = result.FullPath;
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
