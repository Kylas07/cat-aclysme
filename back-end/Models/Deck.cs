namespace CatAclysmeApp.Models
{
    public class Deck
    {
        public int DeckId { get; set; }

        // Le nom du deck
        public required string Name { get; set; }

        // Le joueur qui a crée le deck
        public int PlayerId { get; set; }

        // Relation avec le joueur (peut être définie après la création du joueur)
        public Player? Player { get; set; }

        // Liste des cartes dans le deck
        // public List<Card> Cards { get; set; } = new List<Card>();

    }
}
