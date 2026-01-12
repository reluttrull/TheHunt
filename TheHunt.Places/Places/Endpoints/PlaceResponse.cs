namespace TheHunt.Places.Places.Endpoints
{
    public record PlaceResponse(
        Guid Id,
        string Name,
        Guid LocationId,
        decimal AcceptedRadiusMeters,
        Guid AddedByUserId,
        DateTime AddedDate);
}
