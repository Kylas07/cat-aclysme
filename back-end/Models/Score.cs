namespace CatAclysmeApp.Models
{
    public class Score
    {
        public int ScoreId { get; set; }
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int PlayerId_1 { get; set; }

        public required Game Game { get; set; }
        public required Player Player { get; set; }
        public required Player Player_1 { get; set; }
    }
}
