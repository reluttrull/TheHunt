namespace TheHunt.Places.Places.Endpoints
{
    public record PlaceResponse(
        Guid Id,
        string Name,
        Guid LocationId,
        decimal AcceptedRadiusMeters,
        decimal Latitude,
        decimal Longitude,
        Guid AddedByUserId,
        DateTime AddedDate);
}
