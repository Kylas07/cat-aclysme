namespace CatAclysmeApp.Models
{
    public class GameCard
    {
        public int PlayerId { get; set; }
        public int CardId { get; set; }
        public int GameId { get; set; }
        public int CardPosition { get; set; }
        public int CurrentHealth { get; set; }
        public bool IsActive { get; set; }

        public required Player Player { get; set; }
        public required Card Card { get; set; }
        public required Game Game { get; set; }
    }
}