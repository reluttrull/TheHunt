using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Data;
using TheHunt.Common.Model;
using TheHunt.Places.Locations.Endpoints;
using TheHunt.Places.Places.Endpoints;

namespace TheHunt.Places.Locations
{

    internal class LocationService : ILocationService
    {
        private readonly GameContext _gameContext;

        public LocationService(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public async Task CreateLocationAsync(CreateLocationRequest newLocation)
        {
            var location = new Location
            {
                Id = newLocation.Id!.Value,
                Latitude = newLocation.Latitude,
                Longitude = newLocation.Longitude, // todo: consider standardizing precision early
                RecordedDate = DateTime.UtcNow
            };
            await _gameContext.Locations.AddAsync(location);
            await _gameContext.SaveChangesAsync();
        }

        public Task DeleteLocationAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<LocationResponse?> GetLocationByIdAsync(Guid id)
        {
            var location = await _gameContext.Locations.FindAsync(id);
            if (location is null) return null;
            // todo: tidy up mapping
            return new LocationResponse(location.Id, location.Latitude, location.Longitude, location.RecordedDate);
        }

        public Task<List<LocationResponse>> ListLocationsAsync()
        {
            throw new NotImplementedException();
        }
    }

    public interface ILocationService
    {
        Task<List<LocationResponse>> ListLocationsAsync();
        Task<LocationResponse?> GetLocationByIdAsync(Guid id);
        Task CreateLocationAsync(CreateLocationRequest newLocation);
        Task DeleteLocationAsync(Guid id);
        // todo: update
    }
}
