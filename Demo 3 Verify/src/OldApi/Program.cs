var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/policies/{policyNumber}", (string policyNumber) =>
{
    var policies = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
    {
        ["P-100"] = new
        {
            policyNumber = "P-100",
            holder = "Ada Lovelace",
            product = "Home",
            monthlyPremium = 149.50m,
            status = "Active"
        },
        ["P-200"] = new
        {
            policyNumber = "P-200",
            holder = "Grace Hopper",
            product = "Travel",
            monthlyPremium = 89.00m,
            status = "Pending"
        }
    };

    if (!policies.TryGetValue(policyNumber, out var policy))
    {
        return Results.NotFound(new
        {
            message = "Policy not found",
            policyNumber,
            requestId = Guid.NewGuid(),
            generatedAt = DateTimeOffset.UtcNow
        });
    }

    return Results.Ok(new
    {
        policyNumber = policyNumber,
        holder = policy.GetType().GetProperty("holder")!.GetValue(policy),
        product = policy.GetType().GetProperty("product")!.GetValue(policy),
        monthlyPremium = policy.GetType().GetProperty("monthlyPremium")!.GetValue(policy),
        status = policy.GetType().GetProperty("status")!.GetValue(policy),
        requestId = Guid.NewGuid(),
        generatedAt = DateTimeOffset.UtcNow
    });
})
.WithName("GetPolicy");

app.MapDefaultEndpoints();
app.Run();

public partial class Program { }
