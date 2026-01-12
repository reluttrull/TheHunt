namespace TheHunt.Common.Model
{
    public class Hunt
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PlaceId { get; set; }
        public bool IsHint1Revealed { get; set; }
        public bool IsHint2Revealed { get; set; }
        public bool IsHint3Revealed { get; set; }
        public DateTime? FoundDate { get; set; }
        public Guid? ScoreId { get; set; }
        public bool IsHidden { get; set; }
        public User? User { get; set; }
        public required Place Place { get; set; }
        public Score? Score { get; set; }
    }
}
