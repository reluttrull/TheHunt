using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Text;
using TheHunt.Common.Constants;
using TheHunt.Common.Data;
using TheHunt.Common.Extensions;
using TheHunt.Common.Model;
using TheHunt.Places.Extensions;
using TheHunt.Users.Users;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
        policy.WithOrigins("http://localhost:60388")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
Assembly placesEndpointsAssembly = typeof(TheHunt.Places.Places.Endpoints.Create).Assembly;
Assembly usersEndpointsAssembly = typeof(TheHunt.Users.Users.Endpoints.Register).Assembly;
builder.Services.AddFastEndpoints(options =>
{
    options.Assemblies = [placesEndpointsAssembly, usersEndpointsAssembly];
});

var connectionString = builder.Configuration.GetConnectionString("gamedb");
builder.Services.AddGameDbContext(connectionString!);

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<GameContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? builder.Configuration["JWT_ISSUER"],
            ValidateAudience = true,
            ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? builder.Configuration["JWT_AUDIENCE"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN_SECRET") ?? builder.Configuration["TOKEN_SECRET"]!)),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("access_token"))
                {
                    context.Token = context.Request.Cookies["access_token"];
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy(AuthConstants.AdminUserPolicyName,
        p => p.RequireClaim(AuthConstants.AdminUserClaimName, "true"));
    x.AddPolicy(AuthConstants.PaidMemberUserPolicyName,
        p => p.RequireAssertion(c => c.User.HasClaim(m => m is { Type: AuthConstants.PaidMemberUserClaimName, Value: "true" })
            || c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" })));
    x.AddPolicy(AuthConstants.FreeMemberUserPolicyName,
        p => p.RequireAssertion(c => c.User.HasClaim(m => m is { Type: AuthConstants.FreeMemberUserClaimName, Value: "true" })
            || c.User.HasClaim(m => m is { Type: AuthConstants.PaidMemberUserClaimName, Value: "true" })
            || c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" })));
});

builder.Services.AddPlaceServices(builder.Configuration);
builder.Services.AddUserServices(builder.Configuration);

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

app.UseCors("AllowClient");
app.UseHttpsRedirection();
app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.Request.Method is "POST" or "PUT" or "DELETE")
    {
        if (!context.Request.Headers.ContainsKey("X-CSRF"))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
    }

    await next();
});
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();
