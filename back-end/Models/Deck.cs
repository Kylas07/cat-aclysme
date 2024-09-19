namespace CatAclysmeApp.Models
{
    public class Deck
    {
        public int DeckId { get; set; }
        public required string Name { get; set; }
        public int PlayerId { get; set; }

        public required Player Player { get; set; } // Relation vers Player
    }
}
