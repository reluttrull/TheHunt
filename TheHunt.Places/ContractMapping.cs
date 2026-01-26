using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Model;
using TheHunt.Places.Locations.Endpoints;
using TheHunt.Places.Places.Endpoints;

namespace TheHunt.Places
{
    public static class ContractMapping
    {
        public static LocationResponse MapToResponse(this Location location)
        {
            return new LocationResponse(location.Id, location.Latitude, location.Longitude, location.RecordedDate, location.RecordedByUser);
        }

        public static UnknownPlaceResponse MapToUnknownResponse(this Place place)
        {
            return new UnknownPlaceResponse(place.Id, place.Name, place.AcceptedRadiusMeters, place.AddedByUserId, place.AddedDate);
        }

        public static PlaceResponse MapToResponse(this Place place)
        {
            return new PlaceResponse(place.Id, place.Name, place.LocationId, place.AcceptedRadiusMeters, place.Location?.Latitude ?? 0, place.Location?.Longitude ?? 0, place.AddedByUserId, place.AddedDate);
        }
    }
}
