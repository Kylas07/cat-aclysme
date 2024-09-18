namespace CatAclysmeApp.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public required string Name { get; set; } 
        public required string Password { get; set; }
    }
}