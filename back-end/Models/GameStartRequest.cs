namespace CatAclysmeApp.Models
{
public class GameStartRequest
{
    public required string Player1Pseudo { get; set; } // Le pseudo du joueur 1
    public required string Player2Pseudo { get; set; } // Le pseudo du joueur 2
}
}