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
    var lifts = new Dictionary<int, object>
    {
        [1] = new
        {
            id = 1,
            name = "Gondola A",
            status = "Open",
            waitTimeMinutes = 15
        },
        [4] = new
        {
            id = 4,
            name = "Chairlift South",
            status = "Closed",
            waitTimeMinutes = 0
        }
    };

    if (!lifts.TryGetValue(liftId, out var lift))
    {
        return Results.NotFound(new
        {
            message = "Lift not found",
            liftId,
            requestId = Guid.NewGuid(),
            generatedAt = DateTimeOffset.UtcNow
        });
    }

    return Results.Ok(new
    {
        id = lift.GetType().GetProperty("id")!.GetValue(lift),
        name = lift.GetType().GetProperty("name")!.GetValue(lift),
        status = lift.GetType().GetProperty("status")!.GetValue(lift),
        waitTimeMinutes = lift.GetType().GetProperty("waitTimeMinutes")!.GetValue(lift),
        requestId = Guid.NewGuid(),
        generatedAt = DateTimeOffset.UtcNow
    });
})
.WithName("GetLift");

app.MapDefaultEndpoints();
app.Run();

public partial class Program { }
