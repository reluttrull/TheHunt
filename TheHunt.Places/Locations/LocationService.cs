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

        public async Task CreateLocationAsync(CreateLocationRequest newLocation, CancellationToken token = default)
        {
            var location = new Location
            {
                Id = newLocation.Id!.Value,
                Latitude = newLocation.Latitude,
                Longitude = newLocation.Longitude, // todo: consider standardizing precision early
                RecordedDate = DateTime.UtcNow,
                RecordedByUser = newLocation.RecordedByUser!.Value
            };
            await _gameContext.Locations.AddAsync(location);
            await _gameContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteLocationAsync(Guid id, CancellationToken token = default)
        {
            var locationToDelete = await _gameContext.Locations.FindAsync(id);

            if (locationToDelete is null) return false;

            _gameContext.Locations.Remove(locationToDelete);
            var result = await _gameContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<LocationResponse>> GetAllLocationsForUserAsync(Guid id, CancellationToken token = default)
        {
            return await _gameContext.Locations
                .Where(l => l.RecordedByUser == id)
                .Select(l => new LocationResponse(l.Id, l.Latitude, l.Longitude, l.RecordedDate, l.RecordedByUser))
                .ToListAsync(token);
        }

        public async Task<LocationResponse?> GetLocationByIdAsync(Guid id, CancellationToken token = default)
        {
            var location = await _gameContext.Locations.FindAsync(id);
            if (location is null) return null;
            // todo: tidy up mapping
            return new LocationResponse(location.Id, location.Latitude, location.Longitude, location.RecordedDate, location.RecordedByUser);
        }

        public async Task<IEnumerable<LocationResponse>> GetAllLocationsAsync(GetAllLocationsRequest request, CancellationToken token = default)
        {
            return await _gameContext.Locations
                .Select(l => new LocationResponse(l.Id, l.Latitude, l.Longitude, l.RecordedDate, l.RecordedByUser)) // todo: filter based on request
                .ToListAsync(token);
        }
    }

    public interface ILocationService
    {
        Task<IEnumerable<LocationResponse>> GetAllLocationsAsync(GetAllLocationsRequest request, CancellationToken token = default);
        Task<LocationResponse?> GetLocationByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<LocationResponse>> GetAllLocationsForUserAsync(Guid id, CancellationToken token = default);
        Task CreateLocationAsync(CreateLocationRequest newLocation, CancellationToken token = default);
        Task<bool> DeleteLocationAsync(Guid id, CancellationToken token = default);
        // todo: update
    }
}
