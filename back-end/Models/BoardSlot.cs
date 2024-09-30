namespace CatAclysmeApp.Models
{
    public class BoardSlot
    {
    public int BoardSlotId { get; set; }
    public int Index { get; set; }  // L'emplacement du slot sur le plateau
    public int? CardId { get; set; }  // ID de la carte dans ce slot, null si le slot est vide
    public Card? Card { get; set; }  // La carte dans ce slot
    public int GameId { get; set; }  // Référence à la partie
    public Game Game { get; set; }  // La partie à laquelle ce slot appartient
    }
}