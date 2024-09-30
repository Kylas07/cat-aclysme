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
        public int PlayerId { get; set; } // Player 1
        public int PlayerId_1 { get; set; } // Player 2

        public required Player Player { get; set; } // Relation vers Player (Player 1)
        public required Player Player_1 { get; set; } // Relation vers Player (Player 2)

        public List<BoardSlot> Board { get; set; } = new List<BoardSlot>();

    }

}