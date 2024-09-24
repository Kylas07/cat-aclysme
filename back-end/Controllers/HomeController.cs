using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using CatAclysmeApp.Helpers;
using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Models;
using System.Threading.Tasks;
using System.Linq;

namespace CatAclysmeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly CatAclysmeContext _context;
        private readonly ILogger<HomeController> _logger;

        // Injection de CatAclysmeContext dans le contrôleur
        public HomeController(CatAclysmeContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

         // Inscription d'un utilisateur
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.PlayerName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Le nom et le mot de passe sont requis.");
            }

            // Vérifier si le joueur existe déjà
            var existingPlayer = await _context.Players.SingleOrDefaultAsync(p => p.Name == request.PlayerName);
            if (existingPlayer != null)
            {
                return BadRequest("Le nom d'utilisateur est déjà pris.");
            }

            // Hacher le mot de passe
            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            // Créer un nouvel utilisateur
            var newPlayer = new Player
            {
                Name = request.PlayerName,
                Password = hashedPassword,
                Deck = new Deck
                {
                    Name = $"Deck de {request.PlayerName}",
                    Cards = new List<Card>()
                }
            };

            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            return Ok("Compte créé avec succès.");
        }

        // Connexion d'un utilisateur
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.PlayerName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Le nom et le mot de passe sont requis.");
            }

            // Vérifier si l'utilisateur existe
            var player = await _context.Players.SingleOrDefaultAsync(p => p.Name == request.PlayerName);
            if (player == null)
            {
                return Unauthorized("Utilisateur non trouvé.");
            }

            // Vérifier le mot de passe
            var hashedPassword = PasswordHasher.HashPassword(request.Password);
            if (player.Password != hashedPassword)
            {
                return Unauthorized("Mot de passe incorrect.");
            }

            return Ok("Connexion réussie.");
        }
    

        // Expose un endpoint de test pour vérifier si l'API fonctionne
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { message = "API is running" });
        }

        // Action pour tester l'accès à la base de données
        [HttpGet("test-db")]
        public IActionResult TestDb()
        {
            // Exemple : compter le nombre de joueurs dans la base de données
            var playerCount = _context.Players.Count();
            var cardCount = _context.Cards.Count();

            // Retourner le nombre de joueurs et de cartes sous forme de JSON
            return Ok(new { playerCount, cardCount });
        }

        // Classe pour la requête d'inscription
        public class RegisterRequest
        {
            public required string PlayerName { get; set; }
            public required string Password { get; set; }
        }

        // Classe pour la requête de connexion
        public class LoginRequest
        {
            public required string PlayerName { get; set; }
            public required string Password { get; set; }
        }
    }
}
