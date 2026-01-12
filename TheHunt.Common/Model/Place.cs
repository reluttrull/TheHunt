namespace TheHunt.Common.Model
{
    public class Place
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid LocationId { get; set; }
        public decimal AcceptedRadiusMeters { get; set; }
        public required string Hint1 { get; set; }
        public required string Hint2 { get; set; }
        public required string Hint3 { get; set; }
        public Guid AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }
        public Location? Location { get; set; }
        public User? User { get; set; }
    }
}
