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
    if (!PolicyCatalog.TryGetValue(policyNumber, out var policy))
    {
        return Results.NotFound(new PolicyErrorResponse(
            "Policy not found",
            policyNumber,
            Guid.NewGuid(),
            DateTimeOffset.UtcNow));
    }

    return Results.Ok(new PolicyResponse(
        policy.PolicyNumber,
        policy.Holder,
        policy.Product,
        policy.MonthlyPremium,
        policy.Status,
        Guid.NewGuid(),
        DateTimeOffset.UtcNow));
})
.WithName("GetPolicy");

app.MapDefaultEndpoints();
app.Run();

public partial class Program { }

static class PolicyCatalog
{
    public static readonly IReadOnlyDictionary<string, PolicySnapshot> Policies = new Dictionary<string, PolicySnapshot>(StringComparer.OrdinalIgnoreCase)
    {
        ["P-100"] = new("P-100", "Ada Lovelace", "Home", 149.50m, "Active"),
        ["P-200"] = new("P-200", "Grace Hopper", "Travel", 89.00m, "Pending")
    };

    public static bool TryGetValue(string policyNumber, out PolicySnapshot policy) =>
        Policies.TryGetValue(policyNumber, out policy!);
}

record PolicySnapshot(string PolicyNumber, string Holder, string Product, decimal MonthlyPremium, string Status);
record PolicyResponse(string PolicyNumber, string Holder, string Product, decimal MonthlyPremium, string Status, Guid RequestId, DateTimeOffset GeneratedAt);
record PolicyErrorResponse(string Message, string PolicyNumber, Guid RequestId, DateTimeOffset GeneratedAt);
