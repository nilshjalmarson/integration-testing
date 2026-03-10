var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/lifts/{liftId:int}", (int liftId) =>
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
})
.WithName("GetLift");

app.MapDefaultEndpoints();
app.Run();

public partial class Program { }

static class LiftCatalog
{
    public static readonly IReadOnlyDictionary<int, LiftSnapshot> Lifts = new Dictionary<int, LiftSnapshot>
    {
        [1] = new(1, "Gondola A", "Open", 15),
        [4] = new(4, "Chairlift South", "Closed", 0)
    };

    public static bool TryGetValue(int liftId, out LiftSnapshot lift) =>
        Lifts.TryGetValue(liftId, out lift!);
}

record LiftSnapshot(int Id, string Name, string Status, int WaitTimeMinutes);
record LiftResponse(int Id, string Name, string Status, int WaitTimeMinutes, Guid RequestId, DateTimeOffset GeneratedAt);
record LiftErrorResponse(string Message, int LiftId, Guid RequestId, DateTimeOffset GeneratedAt);
