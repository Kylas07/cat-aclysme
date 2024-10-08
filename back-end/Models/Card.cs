namespace CatAclysmeApp.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public required string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public required string Image { get; set; }
        public required string Description { get; set; }
    }
}