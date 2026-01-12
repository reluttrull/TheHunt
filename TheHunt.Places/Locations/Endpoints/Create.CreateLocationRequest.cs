namespace TheHunt.Places.Locations.Endpoints
{
    public record CreateLocationRequest(
        Guid? Id,
        decimal Latitude,
        decimal Longitude);
}
