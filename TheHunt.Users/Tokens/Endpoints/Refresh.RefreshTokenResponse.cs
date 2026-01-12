namespace TheHunt.Users.Tokens.Endpoints
{
    public record RefreshTokenResponse(string RefreshToken, DateTime Expires);
}
