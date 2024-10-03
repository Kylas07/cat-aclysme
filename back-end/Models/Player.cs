using System.ComponentModel.DataAnnotations;
using back_end.Models;

namespace CatAclysmeApp.Models
{
    public class Player
    {
        public int PlayerId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Le nom doit avoir entre 3 et 20 caractères.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Caractères spéciaux non autorisés.")]
        public required string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 12, ErrorMessage = "Le mot de passe doit contenir entre 12 et 50 caractères.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*]).+$", ErrorMessage = "Le mot de passe doit contenir au moins une majuscule, une minuscule, un chiffre et un caractère spécial.")]
        public required string Password { get; set; }

        // Le deck du joueur (30 cartes)
        public required Deck Deck { get; set; }

        public List<GameDeck> GameDecks { get; set; } = new List<GameDeck>();

    }
}