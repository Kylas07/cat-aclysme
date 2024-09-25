    // Modèles de requêtes
    public class GameStartRequest
    {
        public required string Player1Pseudo { get; set; }
        public required string Player2Pseudo { get; set; }
    }

    public class DrawCardRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }

    public class PlayCardRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int CardId { get; set; }
    }

    public class AttackRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int BoardSlotId { get; set; }  // Emplacement de la carte qui attaque
    }

    public class EndTurnRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }

    public class EndGameRequest
    {
        public int GameId { get; set; }
    }