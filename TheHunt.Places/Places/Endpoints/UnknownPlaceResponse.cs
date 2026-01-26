namespace TheHunt.Places.Places.Endpoints
{
    public record UnknownPlaceResponse(
        Guid Id,
        string Name,
        decimal AcceptedRadiusMeters,
        Guid AddedByUserId,
        DateTime AddedDate);
}
