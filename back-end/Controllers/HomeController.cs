using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using CatAclysmeApp.Helpers;
using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Models;
using System.Threading.Tasks;
using System.Linq;
using back_end.Request;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory; // Pour l'utilisation de IMemoryCache
using System;
using Microsoft.AspNetCore.Antiforgery;

namespace CatAclysmeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly CatAclysmeContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly int _maxAttempts = 5;
        private readonly TimeSpan _lockoutDuration = TimeSpan.FromMinutes(15);
        private readonly IAntiforgery _antiforgery;

        // Injection de CatAclysmeContext, ILogger et IMemoryCache dans le contrôleur
        public HomeController(CatAclysmeContext context, ILogger<HomeController> logger, IMemoryCache memoryCache, IAntiforgery antiforgery)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
            _antiforgery = antiforgery;
        }

        // Inscription d'un utilisateur
        [HttpPost("register")]
        [ValidateAntiForgeryToken] // Protection CSRF
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Validation des champs vides
            if (string.IsNullOrEmpty(request.PlayerName) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogWarning("Tentative d'inscription avec des informations manquantes.");
                return BadRequest("Le nom et le mot de passe sont requis.");
            }

            // Vérifier si le nom d'utilisateur est conforme (entre 3 et 20 caractères et pas de caractères spéciaux)
            if (!Regex.IsMatch(request.PlayerName, @"^[a-zA-Z0-9\s]{3,20}$"))
            {
                _logger.LogWarning($"Nom d'utilisateur non conforme : {request.PlayerName}");
                return BadRequest("Le nom d'utilisateur doit comporter entre 3 et 20 caractères et ne doit contenir que des lettres, chiffres et espaces.");
            }

            // Sanitize the player name to prevent XSS
            string sanitizedPlayerName = HtmlEncoder.Default.Encode(request.PlayerName);

            // Vérifier si le mot de passe est conforme (longueur et complexité)
            if (!Regex.IsMatch(request.Password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*()_+=\-[\]{};':'\\|,.<>/?]).{12,50}$"))
            {
                _logger.LogWarning($"Mot de passe non conforme pour l'utilisateur : {sanitizedPlayerName}");
                return BadRequest("Le mot de passe doit comporter entre 12 et 50 caractères et contenir au moins une majuscule, une minuscule, un chiffre et un caractère spécial.");
            }

            // Vérifier si l'utilisateur existe déjà
            var existingPlayer = await _context.Players.SingleOrDefaultAsync(p => p.Name == sanitizedPlayerName);
            if (existingPlayer != null)
            {
                _logger.LogWarning($"Tentative d'inscription avec un nom déjà pris : {sanitizedPlayerName}");
                return BadRequest("Le nom d'utilisateur est déjà pris.");
            }

            // Hachage du mot de passe
            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            // Créer un nouvel utilisateur
            var newPlayer = new Player
            {
                Name = sanitizedPlayerName,
                Password = hashedPassword,
                Deck = new Deck
                {
                    Name = $"Deck de {sanitizedPlayerName}",
                }
            };

            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Nouvel utilisateur inscrit : {sanitizedPlayerName}");
            return Ok("Compte créé avec succès.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.PlayerName) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogWarning("Tentative de connexion avec des informations manquantes.");
                return BadRequest("Le nom et le mot de passe sont requis.");
            }

            // Générer une clé cache unique basée sur l'IP de l'utilisateur pour suivre les tentatives
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string cacheKey = $"LoginAttempts_{ipAddress}";

            // Vérifier si l'utilisateur est temporairement bloqué
            if (_memoryCache.TryGetValue(cacheKey, out int attempts) && attempts >= _maxAttempts)
            {
                _logger.LogWarning($"Trop de tentatives de connexion pour l'IP : {ipAddress}");
                return BadRequest($"Trop de tentatives de connexion. Veuillez réessayer après {_lockoutDuration.TotalMinutes} minutes.");
            }

            // Sanitize le nom d'utilisateur pour prévenir XSS
            string sanitizedPlayerName = HtmlEncoder.Default.Encode(request.PlayerName);

            // Vérifier si l'utilisateur existe
            var player = await _context.Players.SingleOrDefaultAsync(p => p.Name == sanitizedPlayerName);
            if (player == null)
            {
                _logger.LogWarning($"Tentative de connexion pour un utilisateur non trouvé : {sanitizedPlayerName}");
                IncrémenterTentative(cacheKey);
                return Unauthorized("Utilisateur non trouvé.");
            }

            // Vérification du mot de passe
            var hashedPassword = PasswordHasher.HashPassword(request.Password);
            if (player.Password != hashedPassword)
            {
                _logger.LogWarning($"Échec de la connexion pour l'utilisateur : {sanitizedPlayerName}");
                IncrémenterTentative(cacheKey);
                return Unauthorized("Mot de passe incorrect.");
            }

            // Si connexion réussie, réinitialiser le compteur de tentatives
            _memoryCache.Remove(cacheKey);

            _logger.LogInformation($"Connexion réussie pour l'utilisateur : {sanitizedPlayerName}");
            return Ok(new { message = "Connexion réussie.", playerId = player.PlayerId });
        }

        // Méthode pour incrémenter les tentatives échouées
        private void IncrémenterTentative(string cacheKey)
        {
            // Si la clé existe déjà, on incrémente le compteur, sinon on le crée avec la durée d'expiration
            if (_memoryCache.TryGetValue(cacheKey, out int attempts))
            {
                attempts++;
                _memoryCache.Set(cacheKey, attempts, _lockoutDuration);
            }
            else
            {
                _memoryCache.Set(cacheKey, 1, _lockoutDuration);
            }
        }

        [HttpGet("csrf-token")]
        public IActionResult GetCsrfToken()
        {
            // Generate CSRF tokens and store them in cookies or headers
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });

            return Ok(new { message = "CSRF token generated" });
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
    }
}
