using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Places.Locations;
using TheHunt.Places.Places;

namespace TheHunt.Places.Extensions
{
    public static class PlaceServiceExtensions
    {
        public static IServiceCollection AddPlaceServices(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<ILocationService, LocationService>();
            return services;
        }
    }
}
