using BikeRaceSystem.Services.Localization;

namespace BikeRaceSystem.Tests;

public sealed class LocalizationServiceTests
{
    /// <summary>Localization test: changing language updates a resource and raises one notification.</summary>
    [Fact]
    public void ChangeLanguage_UpdatesResourcesAndRaisesEvent()
    {
        var localization = new LocalizationService(new TestConfigService("hu"));
        var changedCount = 0;
        localization.LanguageChanged += () => changedCount++;

        try
        {
            Assert.Equal("RAJT", localization["StartSignal"]);

            localization.ChangeLanguage("en");

            Assert.Equal("START", localization["StartSignal"]);
            Assert.Equal(1, changedCount);
        }
        finally
        {
            localization.ChangeLanguage("hu");
        }
    }
}
