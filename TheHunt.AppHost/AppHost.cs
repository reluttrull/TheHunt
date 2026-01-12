var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var gamedb = postgres.AddDatabase("gamedb");

var api = builder.AddProject<Projects.TheHunt_Api>("api")
    .WaitFor(gamedb)
    .WithReference(gamedb);

builder.AddExecutable("client", "npm", "../TheHunt.client")
    .WithArgs("run", "start")
    .WithHttpEndpoint(port: 4200, env: "PORT")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
