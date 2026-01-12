namespace TheHunt.Places.Places.Endpoints
{
    public record CreatePlaceRequest(
        Guid? Id,
        string Name,
        Guid LocationId,
        decimal AcceptedRadiusMeters,
        string Hint1,
        string Hint2,
        string Hint3,
        Guid AddedByUserId);
}
