namespace TheHunt.Users.Users.Endpoints
{
    public record RegisterResponse(Guid Id, string Email, string UserName, DateTime JoinedDate);
}
