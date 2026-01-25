namespace TheHunt.Places.Places.Endpoints
{
    public record GetAllPlacesRequest(decimal RequestLatitude, decimal RequestLongitude, Guid? UserId, decimal? MinLatitude, decimal? MaxLatitude, decimal? MinLongitude, decimal? MaxLongitude);
}
