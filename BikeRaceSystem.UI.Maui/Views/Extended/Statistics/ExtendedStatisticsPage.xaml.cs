using BikeRaceSystem.UI.Maui.ViewModels.Race;

namespace BikeRaceSystem.UI.Maui.Views.Extended.Statistics;

public partial class ExtendedStatisticsPage : ContentPage
{
    private bool _scrollDown = true;

    public ExtendedStatisticsPage(ExtendedStatisticsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        Device.StartTimer(TimeSpan.FromMilliseconds(40), () =>
        {
            if (_scrollDown)
            {
                AutoScrollView.ScrollToAsync(0, AutoScrollView.ScrollY + 1, false);
                if (AutoScrollView.ScrollY >= AutoScrollView.ContentSize.Height - AutoScrollView.Height)
                    _scrollDown = false;
            }
            else
            {
                AutoScrollView.ScrollToAsync(0, AutoScrollView.ScrollY - 1, false);
                if (AutoScrollView.ScrollY <= 0)
                    _scrollDown = true;
            }

            return true;
        });
    }
}
