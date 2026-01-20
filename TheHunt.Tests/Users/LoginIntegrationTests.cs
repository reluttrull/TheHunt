using FastEndpoints;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Data;
using TheHunt.Users.Tokens;
using TheHunt.Users.Tokens.Endpoints;
using TheHunt.Users.Users;
using TheHunt.Users.Users.Endpoints;

namespace TheHunt.Tests.Users
{
    [Collection("EF")]
    public class LoginIntegrationTests
    {
        public LoginIntegrationTests()
        {
            Environment.SetEnvironmentVariable("TOKEN_SECRET", "test-secretasdfasdfasdfasdfasdfasdf");
            Environment.SetEnvironmentVariable("JWT_ISSUER", "test-issuer");
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", "test-audience");
        }
        [Fact]
        public async Task Login_WhenValidLogin_LoginSuccessful()
        {
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            await using var context = new GameContext(options);
            var userManager = MockHelpers.BuildUserManager(context);
            var config = NSubstitute.Substitute.For<IConfiguration>();
            var tokenService = new TokenService(context, userManager, config);
            var userService = new UserService(context);
            TheHunt.Users.Users.Endpoints.RegisterRequest regRequest = new("valid@email.com", "valid123", "Abc123.");
            Register regEndpoint = Factory.Create<Register>([userService, userManager]);
            await regEndpoint.HandleAsync(regRequest, CancellationToken.None);
            LoginRequest request = new()
            {
                Email = "valid@email.com",
                Password = "Abc123."
            };
            Login endpoint = Factory.Create<Login>([tokenService, userService, userManager]);

            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.NotNull(endpoint.Response);
            Assert.Equal(200, endpoint.HttpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Login_WhenEmailNotFound_LoginUnsuccessful()
        {
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            await using var context = new GameContext(options);
            var userManager = MockHelpers.BuildUserManager(context);
            var config = NSubstitute.Substitute.For<IConfiguration>();
            var tokenService = new TokenService(context, userManager, config);
            var userService = new UserService(context);
            LoginRequest request = new()
            {
                Email = "different@email.com",
                Password = "Abc123."
            };
            Login endpoint = Factory.Create<Login>([tokenService, userService, userManager]);

            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.NotNull(endpoint.Response);
            Assert.Equal(400, endpoint.HttpContext.Response.StatusCode);
            Assert.NotEmpty(endpoint.ValidationFailures);
            Assert.Equal("Invalid email or password.", endpoint.ValidationFailures[0].ErrorMessage);
        }

        [Fact]
        public async Task Login_WhenPasswordIncorrect_LoginUnsuccessful()
        {
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            await using var context = new GameContext(options);
            var userManager = MockHelpers.BuildUserManager(context);
            var config = NSubstitute.Substitute.For<IConfiguration>();
            var tokenService = new TokenService(context, userManager, config);
            var userService = new UserService(context);
            TheHunt.Users.Users.Endpoints.RegisterRequest regRequest = new("valid@email.com", "valid123", "Abc123.");
            Register regEndpoint = Factory.Create<Register>([userService, userManager]);
            await regEndpoint.HandleAsync(regRequest, CancellationToken.None);
            LoginRequest request = new()
            {
                Email = "valid@email.com",
                Password = "wrongpassword"
            };
            Login endpoint = Factory.Create<Login>([tokenService, userService, userManager]);

            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.NotNull(endpoint.Response);
            Assert.Equal(400, endpoint.HttpContext.Response.StatusCode);
            Assert.NotEmpty(endpoint.ValidationFailures);
            Assert.Equal("Invalid email or password.", endpoint.ValidationFailures[0].ErrorMessage);
        }
    }
}
