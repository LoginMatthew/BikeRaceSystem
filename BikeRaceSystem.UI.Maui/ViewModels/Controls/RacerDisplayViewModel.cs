using BikeRaceSystem.UI.Maui.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BikeRaceSystem.UI.Maui.ViewModels.Controls
{
    public partial class RacerDisplayViewModel : ObservableObject
    {
        [ObservableProperty] private string name;
        [ObservableProperty] private double speed;
        [ObservableProperty] private double progress;
        [ObservableProperty] private string status;
        [ObservableProperty] private int place;
        [ObservableProperty] private bool stopAnimation;
        [ObservableProperty] private bool resetAnimation;
        [ObservableProperty] private bool resetLiquid;
        [ObservableProperty] private RacerState state;


        public bool IsWinner => Place == 1;

        partial void OnPlaceChanged(int value)
        {
            OnPropertyChanged(nameof(IsWinner));
        }

        public void RefreshWinner()
        {
            OnPropertyChanged(nameof(IsWinner));
        }
    }
}
