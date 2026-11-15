using BikeRaceSystem.UI.Maui.ViewModels.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BikeRaceSystem.UI.Maui.ViewModels
{
    public partial class ExtendedRaceViewModel : ObservableObject
    {
        private readonly RaceViewModel _raceVm;

        [ObservableProperty]
        private int countdownValue;

        [ObservableProperty]
        private bool isCountdownVisible;

        [ObservableProperty]
        private bool isStartVisible;

        [ObservableProperty]
        private string startSignalText;

        public RacerDisplayViewModel Racer1 { get; }
        public RacerDisplayViewModel Racer2 { get; }

        public ExtendedRaceViewModel(RaceViewModel raceVm)
        {
            _raceVm = raceVm;
            OnPropertyChanged(nameof(AppTitle));
            OnPropertyChanged(nameof(BackgroundImagePath));

            Racer1 = new RacerDisplayViewModel
            {
                Name = raceVm.Racer1Name
            };

            Racer2 = new RacerDisplayViewModel
            {
                Name = raceVm.Racer2Name
            };

            raceVm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(RaceViewModel.AppTitle))
                    OnPropertyChanged(nameof(AppTitle));

                if (e.PropertyName == nameof(RaceViewModel.BackgroundImagePath))
                    OnPropertyChanged(nameof(BackgroundImagePath));

                if (e.PropertyName == nameof(RaceViewModel.RaceFinished))
                    OnPropertyChanged(nameof(RaceFinished));

                if (e.PropertyName == nameof(RaceViewModel.Racer1Speed))
                    Racer1.Speed = raceVm.Racer1Speed;

                if (e.PropertyName == nameof(RaceViewModel.Racer2Speed))
                    Racer2.Speed = raceVm.Racer2Speed;

                if (e.PropertyName == nameof(RaceViewModel.Racer1Progress))
                    Racer1.Progress = raceVm.Racer1Progress;

                if (e.PropertyName == nameof(RaceViewModel.Racer2Progress))
                    Racer2.Progress = raceVm.Racer2Progress;

                if (e.PropertyName == nameof(RaceViewModel.Racer1Status))
                    Racer1.Status = raceVm.Racer1Status;

                if (e.PropertyName == nameof(RaceViewModel.Racer2Status))
                    Racer2.Status = raceVm.Racer2Status;

                if (e.PropertyName == nameof(RaceViewModel.CountdownValue))
                    CountdownValue = raceVm.CountdownValue;

                if (e.PropertyName == nameof(RaceViewModel.IsCountdownVisible))
                    IsCountdownVisible = raceVm.IsCountdownVisible;

                if (e.PropertyName == nameof(RaceViewModel.IsStartVisible))
                    IsStartVisible = raceVm.IsStartVisible;

                if (e.PropertyName == nameof(RaceViewModel.StartSignalText))
                    StartSignalText = raceVm.StartSignalText;

                if (e.PropertyName == nameof(RaceViewModel.Racer1Name))
                    Racer1.Name = raceVm.Racer1Name;

                if (e.PropertyName == nameof(RaceViewModel.Racer2Name))
                    Racer2.Name = raceVm.Racer2Name;

                if (e.PropertyName == nameof(RaceViewModel.Racer1Place))
                {
                    Racer1.Place = raceVm.Racer1Place;
                    Racer1.RefreshWinner();

                    if (raceVm.Racer1Place == 0)
                        Racer1.ResetLiquid = true;
                }

                if (e.PropertyName == nameof(RaceViewModel.Racer2Place))
                {
                    Racer2.Place = raceVm.Racer2Place;
                    Racer2.RefreshWinner();

                    if (raceVm.Racer2Place == 0)
                        Racer2.ResetLiquid = true;
                }

                if (e.PropertyName == nameof(RaceViewModel.RaceFinished))
                {
                    Racer1.StopAnimation = _raceVm.RaceFinished;
                    Racer2.StopAnimation = _raceVm.RaceFinished;
                }
            };
        }

        public string AppTitle => _raceVm.AppTitle;
        public string BackgroundImagePath => _raceVm.BackgroundImagePath;
        public bool RaceFinished => _raceVm.RaceFinished;
    }
}
