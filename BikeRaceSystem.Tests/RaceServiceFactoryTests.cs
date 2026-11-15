using BikeRaceSystem.Services.Factories;
using BikeRaceSystem.Services.Race;
using Microsoft.Extensions.DependencyInjection;

namespace BikeRaceSystem.Tests;

public sealed class RaceServiceFactoryTests
{
    /// <summary>Factory test: a missing race-mode setting must select the demo service.</summary>
    [Fact]
    public void GetService_UsesRandomServiceWhenModeIsNotConfigured()
    {
        var randomService = new RandomRaceService();
        var sensorService = CreateSensorService();
        using var provider = CreateProvider(randomService, sensorService);
        var factory = new RaceServiceFactory(provider, new TestConfigService());

        Assert.Same(randomService, factory.GetService());
    }

    /// <summary>Factory test: the saved Sensor mode must select the sensor-backed service.</summary>
    [Fact]
    public void GetService_UsesSensorServiceWhenSensorModeIsSelected()
    {
        var randomService = new RandomRaceService();
        var sensorService = CreateSensorService();
        using var provider = CreateProvider(randomService, sensorService);
        var factory = new RaceServiceFactory(provider, new TestConfigService("Sensor"));

        Assert.Same(sensorService, factory.GetService());
    }

    private static SensorRaceService CreateSensorService() =>
        new(new TestAppConfigService(), new UnusedSensorTransportFactory());

    private static ServiceProvider CreateProvider(RandomRaceService random, SensorRaceService sensor) =>
        new ServiceCollection()
            .AddSingleton(random)
            .AddSingleton(sensor)
            .BuildServiceProvider();
}
