namespace CatAclysmeApp.Models
{
    public class Turn
    {
        public int TurnId { get; set; }
        public int GameId { get; set; }
        public int CardId { get; set; }
        public int CardId_1 { get; set; }
        public int PlayerId { get; set; }

        public required Game Game { get; set; }
        public required Card Card { get; set; }
        public required Card Card_1 { get; set; }
        public required Player Player { get; set; }
    }
}