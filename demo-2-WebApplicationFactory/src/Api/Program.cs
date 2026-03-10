using Demo2.Api.Data;
using Demo2.Api.Endpoints;
using Demo2.Api.Weather;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();

builder.Services.AddSingleton<LiftRepository>();
builder.Services.AddHttpClient<WeatherClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WeatherApi:BaseUrl"] ?? "https://api.open-meteo.com");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapLiftEndpoints();

app.MapDefaultEndpoints();
app.Run();
