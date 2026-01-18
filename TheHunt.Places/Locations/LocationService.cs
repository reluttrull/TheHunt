using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Data;
using TheHunt.Common.Extensions;
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
                Longitude = newLocation.Longitude,
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

        public async Task<IEnumerable<Location>> GetAllLocationsForUserAsync(Guid id, CancellationToken token = default)
        {
            return await _gameContext.Locations
                .Where(l => l.RecordedByUser == id)
                .OrderByDescending(l => l.RecordedDate)
                .ToListAsync(token);
        }

        public async Task<Location?> GetLocationByIdAsync(Guid id, CancellationToken token = default)
        {
            var location = await _gameContext.Locations.FindAsync(id);
            if (location is null) return null;
            
            return location;
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync(GetAllLocationsRequest request, CancellationToken token = default)
        {
            return await _gameContext.Locations
                .WhereIf(request.UserId is not null, l => l.RecordedByUser == request.UserId)
                .WhereIf(request.MinLatitude is not null, l => l.Latitude >= request.MinLatitude)
                .WhereIf(request.MaxLatitude is not null, l => l.Latitude <= request.MaxLatitude)
                .WhereIf(request.MinLongitude is not null, l => l.Longitude >= request.MinLongitude)
                .WhereIf(request.MaxLongitude is not null, l => l.Longitude <= request.MaxLongitude)
                .OrderByDescending(l => l.RecordedDate)
                .ToListAsync(token);
        }
    }

    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync(GetAllLocationsRequest request, CancellationToken token = default);
        Task<Location?> GetLocationByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Location>> GetAllLocationsForUserAsync(Guid id, CancellationToken token = default);
        Task CreateLocationAsync(CreateLocationRequest newLocation, CancellationToken token = default);
        Task<bool> DeleteLocationAsync(Guid id, CancellationToken token = default);
        // todo: update
    }
}
