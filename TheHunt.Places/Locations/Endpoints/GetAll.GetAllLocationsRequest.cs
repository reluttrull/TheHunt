namespace TheHunt.Places.Locations.Endpoints
{
    public record GetAllLocationsRequest(Guid? UserId, decimal? MinLatitude, decimal? MaxLatitude, decimal? MinLongitude, decimal? MaxLongitude);
}
