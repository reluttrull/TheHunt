using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TheHunt.Common.Data;
using TheHunt.Common.Model;
using TheHunt.Users.Users;
using TheHunt.Users.Users.Endpoints;

namespace TheHunt.Tests.Users
{
    [Collection("EF")]
    public class RegistrationIntegrationTests
    {
        [Fact]
        public async Task Register_WhenValidUserData_RegistrationSuccessful()
        {
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            await using var context = new GameContext(options); 
            var userManager = MockHelpers.BuildUserManager(context);
            var userService = new UserService(context);
            RegisterRequest request = new("valid@email.com", "valid123", "Abc123.");
            Register endpoint = Factory.Create<Register>([userService, userManager]);

            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.NotNull(endpoint.Response);
            Assert.Equal(201, endpoint.HttpContext.Response.StatusCode);
            Assert.Equal(1, context.Users.Count());
        }

        [Fact]
        public async Task Register_WhenDuplicateEmail_RegistrationUnsuccessful()
        {
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            await using var context = new GameContext(options);
            var userManager = MockHelpers.BuildUserManager(context);
            var userService = new UserService(context);
            RegisterRequest request1 = new("valid@email.com", "valid123", "Abc123.");
            RegisterRequest request2 = new("valid@email.com", "anotheruser", "Abc123?");
            Register endpoint = Factory.Create<Register>([userService, userManager]);

            await endpoint.HandleAsync(request1, CancellationToken.None);
            await endpoint.HandleAsync(request2, CancellationToken.None);

            Assert.NotNull(endpoint.Response);
            Assert.Equal(400, endpoint.HttpContext.Response.StatusCode);
            Assert.Equal(1, context.Users.Count());
        }

        [Fact]
        public async Task Register_WhenDuplicateUserName_RegistrationUnsuccessful()
        {
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            await using var context = new GameContext(options);
            var userManager = MockHelpers.BuildUserManager(context);
            var userService = new UserService(context);
            RegisterRequest request1 = new("valid@email.com", "valid123", "Abc123.");
            RegisterRequest request2 = new("different@email.com", "valid123", "Abc123?");
            Register endpoint = Factory.Create<Register>([userService, userManager]);

            await endpoint.HandleAsync(request1, CancellationToken.None);
            await endpoint.HandleAsync(request2, CancellationToken.None);

            Assert.NotNull(endpoint.Response);
            Assert.Equal(400, endpoint.HttpContext.Response.StatusCode);
            Assert.Equal(1, context.Users.Count());
        }

        [Fact]
        public async Task Register_WhenPasswordDoesNotMeetRequirements_RegistrationUnsuccessful()
        {
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            await using var context = new GameContext(options);
            var userManager = MockHelpers.BuildUserManager(context);
            var userService = new UserService(context);
            RegisterRequest request = new("valid@email.com", "valid123", "password");
            Register endpoint = Factory.Create<Register>([userService, userManager]);

            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.NotNull(endpoint.Response);
            Assert.Equal(400, endpoint.HttpContext.Response.StatusCode);
            Assert.Equal(0, context.Users.Count());
        }
    }
}
