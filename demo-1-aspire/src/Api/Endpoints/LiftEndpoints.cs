using Demo1.Api.Data;

namespace Demo1.Api.Endpoints;

public static class LiftEndpoints
{
    public static void MapLiftEndpoints(this WebApplication app)
    {
        app.MapGet("/lifts", GetAllLifts)
            .WithName("GetLifts")
            .RequireAuthorization();
    }

    private static async Task<IEnumerable<object>> GetAllLifts(LiftRepository repository)
    {
        var lifts = await repository.GetAllLifts();
        return lifts.Select(lift => new
        {
            lift.Id,
            lift.Name,
            lift.Status,
            lift.WaitTimeMinutes
        });
    }
}
