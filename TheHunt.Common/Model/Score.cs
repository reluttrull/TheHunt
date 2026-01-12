namespace TheHunt.Common.Model
{
    public class Score
    {
        public Guid Id { get; set; }
        public Guid EarnedByUserId { get; set; }
        public DateTime EarnedDate { get; set; }
        public int Points { get; set; }
    }
}
