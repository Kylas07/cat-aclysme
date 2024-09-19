namespace CatAclysmeApp.Models
{
    public class PlayerHand
    {
        public int PlayerHandId { get; set; }
        public int PositionInHand { get; set; }
        public int CardAmount { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int CardId { get; set; }

        public required Game Game { get; set; }
        public required Player Player { get; set; }
        public required Card Card { get; set; }
    }
}
