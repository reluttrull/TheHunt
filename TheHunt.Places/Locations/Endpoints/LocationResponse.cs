namespace TheHunt.Places.Locations.Endpoints
{
    public record LocationResponse(
        Guid Id,
        decimal Latitude,
        decimal Longitude,
        DateTime RecordedDate);
}
