using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using CatAclysmeApp.Helpers;
using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Models;
using System.Threading.Tasks;
using System.Linq;
using back_end.Request;

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
                _logger.LogWarning("Tentative d'inscription avec des informations manquantes.");
                return BadRequest("Le nom et le mot de passe sont requis.");
            }

            // Vérifier si le joueur existe déjà
            var existingPlayer = await _context.Players.SingleOrDefaultAsync(p => p.Name == request.PlayerName);
            if (existingPlayer != null)
            {
                _logger.LogWarning($"Tentative d'inscription avec un nom déjà pris : {request.PlayerName}");
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
                }
            };

            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();
            var builds = _context.Cards.Select(card => new Build
            {
                Deck = newPlayer.Deck,
                DeckId = newPlayer.Deck.DeckId,
                Card = card,
                CardId = card.CardId,
                Amount = 1
            }).ToList();

            _context.Builds.AddRange(builds);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Nouvel utilisateur inscrit : {request.PlayerName}");
            return Ok("Compte créé avec succès.");
        }

        // Connexion d'un utilisateur
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.PlayerName) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogWarning("Tentative de connexion avec des informations manquantes.");
                return BadRequest("Le nom et le mot de passe sont requis.");
            }

            // Vérifier si l'utilisateur existe
            var player = await _context.Players.SingleOrDefaultAsync(p => p.Name == request.PlayerName);
            if (player == null)
            {
                _logger.LogWarning($"Tentative de connexion pour un utilisateur non trouvé : {request.PlayerName}");
                return Unauthorized("Utilisateur non trouvé.");
            }

            // Vérifier le mot de passe
            var hashedPassword = PasswordHasher.HashPassword(request.Password);
            if (player.Password != hashedPassword)
            {
                _logger.LogWarning($"Échec de la connexion pour l'utilisateur : {request.PlayerName}");
                return Unauthorized("Mot de passe incorrect.");
            }

            _logger.LogInformation($"Connexion réussie pour l'utilisateur : {request.PlayerName}");

            // Retourner l'ID du joueur avec le message de succès
            return Ok(new { message = "Connexion réussie.", playerId = player.PlayerId });
        }


        // Expose un endpoint de test pour vérifier si l'API fonctionne
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Test de ping effectué avec succès.");
            return Ok(new { message = "API is running" });
        }

        // Action pour tester l'accès à la base de données
        [HttpGet("test-db")]
        public IActionResult TestDb()
        {
            // Exemple : compter le nombre de joueurs dans la base de données
            var playerCount = _context.Players.Count();
            var cardCount = _context.Cards.Count();

            _logger.LogInformation("Accès à la base de données effectué. Nombre de joueurs et de cartes retourné.");

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
