var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();

builder.Services.AddHttpClient("FruitApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["FruitApi:BaseUrl"] ?? "https://www.fruityvice.com");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapGet("/fruits", async (IHttpClientFactory httpClientFactory, CancellationToken cancellationToken) =>
{
    var client = httpClientFactory.CreateClient("FruitApi");
    var response = await client.GetAsync("/api/fruit/all", cancellationToken);

    if (!response.IsSuccessStatusCode)
        return Results.StatusCode((int)response.StatusCode);

    var fruits = await response.Content.ReadFromJsonAsync<object[]>(cancellationToken);
    return Results.Ok(fruits);
})
.WithName("GetFruits");

app.MapDefaultEndpoints();
app.Run();

public partial class Program { }
