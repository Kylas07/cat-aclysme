namespace CatAclysmeApp.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public int Player1HP { get; set; }
        public int Player2HP { get; set; }
        public int PlayerTurn { get; set; }
        public int TurnCount { get; set; }
        public int GameStatus { get; set; }

        // Propriétés pour les decks des joueurs
        public List<Card> Player1Deck { get; set; } = new List<Card>();
        public List<Card> Player2Deck { get; set; } = new List<Card>();

        // Propriétés pour les mains des joueurs
        public List<Card> Player1Hand { get; set; } = new List<Card>();
        public List<Card> Player2Hand { get; set; } = new List<Card>();

        public int PlayerId { get; set; }
        public int PlayerId_1 { get; set; }

        public required Player Player { get; set; }
        public required Player Player_1 { get; set; }

        // Propriété pour les cartes sur le plateau
        public List<Card> CardsOnBoard { get; set; } = new List<Card>();
    }

}