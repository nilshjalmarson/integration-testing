var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Api>("Api")
    .WithEnvironment("FruitApi__BaseUrl", "https://www.fruityvice.com");

builder.Build().Run();
