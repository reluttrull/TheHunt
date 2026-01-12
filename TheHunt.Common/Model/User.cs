using Microsoft.AspNetCore.Identity;

namespace TheHunt.Common.Model
{
    public class User : IdentityUser<Guid>
    {
        public Guid? LastLocationId { get; set; }
        public Location? LastLocation { get; set; }
        public List<Score> Scores { get; set; } = [];
        public List<UserBadge> UserBadges { get; set; } = [];
        public DateTime JoinedDate { get; init; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
        public DateTime LastEditedDate { get; set; }
    }
}
