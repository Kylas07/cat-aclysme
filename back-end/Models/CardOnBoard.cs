public class CardOnBoard
{
    public int CardOnBoardId { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; } // Référence à la partie

    public int CardId { get; set; }
    public Card Card { get; set; } // La carte posée

    public int PlayerId { get; set; }
    public Player Player { get; set; } // Le joueur qui a posé la carte

    public int Position { get; set; } // La position de la carte sur le plateau (optionnel)
}
