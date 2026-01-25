using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Data;
using TheHunt.Common.Extensions;
using TheHunt.Common.Model;
using TheHunt.Places.Places.Endpoints;

namespace TheHunt.Places.Places
{

    internal class PlaceService : IPlaceService
    {
        private readonly GameContext _gameContext;

        public PlaceService(GameContext gameContext)
        {
            _gameContext = gameContext; 
        }
        public async Task CreatePlaceAsync(CreatePlaceRequest newPlace, CancellationToken token = default)
        {
            var place = new Place
            {
                Id = newPlace.Id!.Value,
                Name = newPlace.Name,
                LocationId = newPlace.LocationId,
                AcceptedRadiusMeters = newPlace.AcceptedRadiusMeters,
                Hint1 = newPlace.Hint1,
                Hint2 = newPlace.Hint2,
                Hint3 = newPlace.Hint3,
                AddedByUserId = newPlace.AddedByUserId!.Value,
                AddedDate = DateTime.UtcNow
            };
            await _gameContext.Places.AddAsync(place);
            await _gameContext.SaveChangesAsync();
        }

        public async Task<bool> DeletePlaceAsync(Guid id, CancellationToken token = default)
        {
            var placeToDelete = await _gameContext.Places.FindAsync(id);

            if (placeToDelete is null) return false;

            _gameContext.Places.Remove(placeToDelete);
            var result = await _gameContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Place?> GetPlaceByIdAsync(Guid id, CancellationToken token = default)
        {
            var place = await _gameContext.Places
                .Include(p => p.Location)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (place is null) return null;

            return place;
        }

        public async Task<List<Place>> GetAllPlacesAsync(GetAllPlacesRequest req, CancellationToken token = default)
        {
            return await _gameContext.Places
                .Include(p => p.Location)
                .WhereIf(req.UserId is not null, p => p.AddedByUserId == req.UserId)
                .WhereIf(req.MinLatitude is not null, p => p.Location != null && p.Location!.Latitude >= req.MinLatitude)
                .WhereIf(req.MaxLatitude is not null, p => p.Location != null && p.Location!.Latitude <= req.MaxLatitude)
                .WhereIf(req.MinLongitude is not null, p => p.Location != null && p.Location!.Longitude >= req.MinLongitude)
                .WhereIf(req.MaxLongitude is not null, p => p.Location != null && p.Location!.Longitude <= req.MaxLongitude)
                .OrderBy(p => p.Location == null 
                    ? decimal.MaxValue 
                    : Math.Abs(req.RequestLatitude - p.Location.Latitude) + Math.Abs(req.RequestLongitude - p.Location.Longitude))
                .ToListAsync(token);
        }
        public async Task<List<Place>> GetAllPlacesForUserAsync(Guid id, CancellationToken token = default)
        {
            return await _gameContext.Places
                .Where(p => p.Id == id)
                .Include(p => p.Location)
                .ToListAsync();
        }

        public async Task<Place?> UpdatePlaceAsync(Guid id, UpdatePlaceRequest req, CancellationToken token = default)
        {
            var placeToChange = await _gameContext.Places.FindAsync(id);
            if (placeToChange is null) return null;

            placeToChange.Name = req.Name;
            placeToChange.LocationId = req.LocationId;
            placeToChange.AcceptedRadiusMeters = req.AcceptedRadiusMeters;
            placeToChange.Hint1 = req.Hint1;
            placeToChange.Hint2 = req.Hint2;
            placeToChange.Hint3 = req.Hint3;

            await _gameContext.SaveChangesAsync(token);

            return placeToChange;
        }
    }

    public interface IPlaceService
    {
        Task<List<Place>> GetAllPlacesAsync(GetAllPlacesRequest req, CancellationToken token = default);
        Task<List<Place>> GetAllPlacesForUserAsync(Guid id, CancellationToken token = default);
        Task<Place?> GetPlaceByIdAsync(Guid id, CancellationToken token = default);
        Task CreatePlaceAsync(CreatePlaceRequest newPlace, CancellationToken token = default);
        Task<Place?> UpdatePlaceAsync(Guid id, UpdatePlaceRequest req, CancellationToken token = default);
        Task<bool> DeletePlaceAsync(Guid id, CancellationToken token = default);
    }
}
