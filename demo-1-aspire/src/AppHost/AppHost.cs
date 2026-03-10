var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql");

var skiresort = sql.AddDatabase("skiresort")
    .WithCreationScript("""
        IF DB_ID('skiresort') IS NULL
            CREATE DATABASE [skiresort];
        GO

        USE [skiresort];
        GO

        -- Create the Lift table
        CREATE TABLE dbo.Lift (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Name NVARCHAR(255) NOT NULL,
            Status NVARCHAR(50) NOT NULL,
            WaitTimeMinutes INT NOT NULL
        );
        GO

        -- Insert sample lift data
        INSERT INTO dbo.Lift (Name, Status, WaitTimeMinutes) VALUES
            ('Gondola A', 'Open', 15),
            ('T-Bar 1', 'Open', 5),
            ('Chairlift North', 'Open', 25),
            ('Chairlift South', 'Closed', 0),
            ('Express Lift East', 'Open', 12),
            ('Surface Lift West', 'Maintenance', 0),
            ('Alpine Peak Gondola', 'Open', 30);
        GO
        """);

builder.AddProject<Projects.Api>("Api")
    .WithReference(skiresort)
    .WaitFor(skiresort);

builder.Build().Run();
