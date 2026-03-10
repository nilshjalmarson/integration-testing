var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Api>("Api")
    .WithEnvironment("WeatherApi__BaseUrl", "https://api.open-meteo.com");

builder.Build().Run();
