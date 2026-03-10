using Api.Data;
using Api.Weather;

namespace Api.Endpoints;

public static class LiftEndpoints
{
    public static void MapLiftEndpoints(this WebApplication app)
    {
        app.MapGet("/lifts", GetAllLifts)
            .WithName("GetLifts");
    }

    private static async Task<IEnumerable<object>> GetAllLifts(
        LiftRepository repository,
        WeatherClient weatherClient,
        CancellationToken cancellationToken)
    {
        var lifts = await repository.GetAllLifts();
        var currentTemperatureCelsius = await weatherClient.GetCurrentTemperatureCelsius(cancellationToken);
        var closeAllLifts = currentTemperatureCelsius is < -19;

        return lifts.Select(lift => new
        {
            lift.Id,
            lift.Name,
            Status = closeAllLifts ? "Closed" : lift.Status,
            WaitTimeMinutes = closeAllLifts ? 0 : lift.WaitTimeMinutes
        });
    }
}
