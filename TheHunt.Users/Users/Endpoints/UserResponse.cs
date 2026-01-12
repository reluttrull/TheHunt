namespace TheHunt.Users.Users.Endpoints
{
    public record UserResponse(Guid Id, string Email, string UserName, DateTime JoinedDate);
}
