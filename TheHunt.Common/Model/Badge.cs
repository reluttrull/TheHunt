namespace TheHunt.Common.Model
{
    public class Badge
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string ImageUrl { get; set; }
    }
}
