using BikeRaceSystem.UI.Maui.ViewModels.Controls;

namespace BikeRaceSystem.UI.Maui.Views.Controls;

public partial class RacerDisplay : ContentView
{
    private bool _isAlive = true;

    public RacerDisplay()
    {
        InitializeComponent();

        // 60 FPS animation
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
        {
            if (!_isAlive)
                return false;

            if (BindingContext is RacerDisplayViewModel vm)
            {
                if (vm.ResetLiquid)
                {
                    Liquid.HeightRequest = 0;
                    vm.ResetLiquid = false;
                }

                if (!vm.StopAnimation)
                {
                    AnimateLiquid(vm.Progress);
                }
            }

            return true;
        });

    }
 
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is RacerDisplayViewModel vm)
        {
            Liquid.HeightRequest = vm.Progress * 400;
        }
    }

    protected override void OnParentChanged()
    {
        base.OnParentChanged();

        if (Parent == null)
        {
            _isAlive = false;
        }
    }

    private void AnimateLiquid(double progress)
    {
        double maxHeight = 400;
        double targetHeight = progress * maxHeight;

        Liquid.HeightRequest = Liquid.HeightRequest + (targetHeight - Liquid.HeightRequest) * 0.15;
    }

    public void ResetLiquid()
    {
        Liquid.HeightRequest = 0;
    }

}
