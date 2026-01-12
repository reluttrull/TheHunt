using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using TheHunt.Common.Data;
using TheHunt.Common.Extensions;
using TheHunt.Places.Extensions;
using TheHunt.Places.Places.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
Assembly endpointsAssembly = typeof(Create).Assembly;
builder.Services.AddFastEndpoints(options =>
{
    options.Assemblies = [endpointsAssembly];
});

var connectionString = builder.Configuration.GetConnectionString("gamedb");
builder.Services.AddGameDbContext(connectionString!);

builder.Services.AddPlaceServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<GameContext>();
    db.Database.Migrate();
}


app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();

app.Run();
