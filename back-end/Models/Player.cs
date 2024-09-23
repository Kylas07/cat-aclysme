namespace CatAclysmeApp.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public required string Name { get; set; } 

        // Le deck du joueur (30 cartes)
        public required Deck Deck { get; set; }

        // La main du joueur (aucune limite imposée ici)
        public List<PlayerHand> Hand { get; set; } = new List<PlayerHand>();
    }
}