namespace CatAclysmeApp.Models
{
    public class Build
    {
        public int DeckId { get; set; }
        public int CardId { get; set; }
        public int Amount { get; set; }

        public required Deck Deck { get; set; }
        public required Card Card { get; set; }
    }
}
