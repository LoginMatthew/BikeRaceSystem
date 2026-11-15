using BikeRaceSystem.UI.Maui.ViewModels;

namespace BikeRaceSystem.UI.Maui.Views.Pages;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void OnHomeSizeChanged(object? sender, EventArgs e)
    {
        var width = Width;
        if (width <= 0)
            return;

        HomeLayout.WidthRequest = Math.Min(1180, width);

        var compact = width < 760;
        var narrow = width < 520;

        HomeLayout.Padding = narrow
            ? new Thickness(16, 18)
            : compact
                ? new Thickness(22, 22)
                : new Thickness(36, 28);
        HomeLayout.RowSpacing = narrow ? 18 : 26;
        HeroTitle.FontSize = narrow ? 27 : compact ? 30 : 34;
        StatusPill.IsVisible = !narrow;

        HeroLayout.ColumnDefinitions.Clear();
        HeroLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        HeroLayout.ColumnSpacing = compact ? 0 : 20;
        HeroArt.IsVisible = !compact;
        if (!compact)
            HeroLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

        ActionsLayout.ColumnDefinitions.Clear();
        ActionsLayout.RowDefinitions.Clear();
        if (compact)
        {
            ActionsLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            ActionsLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            ActionsLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            ActionsLayout.RowSpacing = 14;
            Grid.SetRow(StatisticsCard, 0);
            Grid.SetColumn(StatisticsCard, 0);
            Grid.SetRow(SettingsCard, 1);
            Grid.SetColumn(SettingsCard, 0);
        }
        else
        {
            ActionsLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            ActionsLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            ActionsLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            ActionsLayout.ColumnSpacing = 18;
            Grid.SetRow(StatisticsCard, 0);
            Grid.SetColumn(StatisticsCard, 0);
            Grid.SetRow(SettingsCard, 0);
            Grid.SetColumn(SettingsCard, 1);
        }
    }
}
