using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Data;
using TheHunt.Common.Model;

namespace TheHunt.Tests
{
    public static class MockHelpers
    {
        public static UserManager<User> BuildUserManager(GameContext context)
        {
            var store = new UserStore<User, IdentityRole<Guid>, GameContext, Guid>(context);

            var options = Options.Create(new IdentityOptions());

            var passwordHasher = new PasswordHasher<User>();

            var userValidators = new List<IUserValidator<User>>
            {
                new UserValidator<User>()
            };

            var passwordValidators = new List<IPasswordValidator<User>>
            {
                new PasswordValidator<User>()
            };

            var lookupNormalizer = new UpperInvariantLookupNormalizer();

            var errorDescriber = new IdentityErrorDescriber();

            var services = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var logger = services.GetRequiredService<ILogger<UserManager<User>>>();

            return new UserManager<User>(
                store,
                options,
                passwordHasher,
                userValidators,
                passwordValidators,
                lookupNormalizer,
                errorDescriber,
                services,
                logger
            );
        }
    }
}
