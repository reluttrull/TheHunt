using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Data;

namespace TheHunt.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGameDbContext(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<GameContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(GameContext).Assembly.FullName);
                });
            });

            return services;
        }
    }
}
