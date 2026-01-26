namespace TheHunt.Places.Places.Endpoints
{
    public record GetAllPlacesForUserRequest(decimal RequestLatitude, decimal RequestLongitude, decimal? MinLatitude, decimal? MaxLatitude, decimal? MinLongitude, decimal? MaxLongitude);
}
