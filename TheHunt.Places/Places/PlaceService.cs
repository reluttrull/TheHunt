using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Data;
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
        public async Task CreatePlaceAsync(CreatePlaceRequest newPlace)
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

        public Task DeletePlaceAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PlaceResponse?> GetPlaceByIdAsync(Guid id)
        {
            var place = await _gameContext.Places.FindAsync(id);
            if (place is null) return null;
            // todo: tidy up mapping
            return new PlaceResponse(place.Id, place.Name, place.LocationId, 
                place.AcceptedRadiusMeters, place.AddedByUserId, place.AddedDate);
        }

        public Task<List<PlaceResponse>> ListPlacesAsync()
        {
            throw new NotImplementedException();
        }
    }

    public interface IPlaceService
    {
        Task<List<PlaceResponse>> ListPlacesAsync();
        Task<PlaceResponse?> GetPlaceByIdAsync(Guid id);
        Task CreatePlaceAsync(CreatePlaceRequest newPlace);
        Task DeletePlaceAsync(Guid id);
        // todo: update
    }
}
