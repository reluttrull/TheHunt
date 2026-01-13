using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Users.Tokens;
using TheHunt.Users.Users;

namespace TheHunt.Places.Extensions
{
    public static class UserServiceExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
