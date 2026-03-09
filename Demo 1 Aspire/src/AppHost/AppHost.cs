var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Api>("Api");

builder.Build().Run();
