namespace TheHunt.Common.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid? LastLocationId { get; set; }
        public Location? LastLocation { get; set; }
        public List<Score> Scores { get; set; } = [];
        public List<UserBadge> UserBadges { get; set; } = [];
    }
}
