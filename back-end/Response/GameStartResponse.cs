using back_end.Models;
using CatAclysmeApp.Models;

namespace back_end.Response
{
    public class GameStartResponse
    {
        public required Game Game { get; set; }
        public required List<GameDeck> Player1GameDeck { get; set; }
        public required List<GameDeck> Player2GameDeck { get; set; }
    }
}
