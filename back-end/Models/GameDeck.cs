using back_end.Enums;
using CatAclysmeApp.Models;

namespace back_end.Models
{
    public class GameDeck
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public int CardId { get; set; }
        public int CardOrder { get; set; }
        public CardState CardState { get; set; }

        public required Player Player { get; set; }
        public required Card Card { get; set; }
        public required Game Game { get; set; }

        public bool IsPlacedPreviousTurn { get; set; } = false; // Si la carte a été posée lors du tour précédent
        public bool HasAttackedThisTurn { get; set; } = false; // Si la carte a attaqué ce tour
    }
}
