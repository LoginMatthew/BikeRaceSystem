using BikeRaceSystem.UI.Maui.ViewModels;

namespace BikeRaceSystem.UI.Maui.Views.Pages;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage(StatisticsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}