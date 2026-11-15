using BikeRaceSystem.Core.Models;

namespace BikeRaceSystem.Core.Interfaces.Race;

public interface ISensorTransportFactory
{
    Task<Stream> ConnectAsync(SensorEndpointConfig endpoint, CancellationToken cancellationToken);
}
