namespace TheHunt.Places.Places.Endpoints
{
    public record GetAllPlacesRequest(Guid? UserId, decimal? MinLatitude, decimal? MaxLatitude, decimal? MinLongitude, decimal? MaxLongitude);
}
