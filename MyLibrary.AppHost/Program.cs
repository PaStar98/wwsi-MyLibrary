using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql-server")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithDbGate();

var db = sql.AddDatabase("mylibrary-db");

var api = builder.AddProject<MyLibrary_Api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();