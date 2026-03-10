var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.OldApi>("old");
builder.AddProject<Projects.NewApi>("new");

builder.Build().Run();
