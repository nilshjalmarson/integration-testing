static class LiftEndpoints
{
    private static readonly IReadOnlyDictionary<int, LiftSnapshot> LiftCatalog = new Dictionary<int, LiftSnapshot>
    {
        [1] = new(1, "Gondola A", "Open", 15),
        [4] = new(4, "Chairlift South", "Closed", 0)
    };

    public static IEndpointRouteBuilder MapLiftEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/lifts/{liftId:int}", GetLift)
            .WithName("GetLift");

        return app;
    }

    private static IResult GetLift(int liftId)
    {
        if (!LiftCatalog.TryGetValue(liftId, out var lift))
        {
            return Results.NotFound(new LiftErrorResponse(
                "Lift not found",
                liftId,
                Guid.NewGuid(),
                DateTimeOffset.UtcNow));
        }

        return Results.Ok(new LiftResponse(
            lift.Id,
            lift.Name,
            lift.Status,
            lift.WaitTimeMinutes,
            Guid.NewGuid(),
            DateTimeOffset.UtcNow));
    }

    private sealed record LiftSnapshot(int Id, string Name, string Status, int WaitTimeMinutes);
    private sealed record LiftResponse(int Id, string Name, string Status, int WaitTimeMinutes, Guid RequestId, DateTimeOffset GeneratedAt);
    private sealed record LiftErrorResponse(string Message, int LiftId, Guid RequestId, DateTimeOffset GeneratedAt);
}