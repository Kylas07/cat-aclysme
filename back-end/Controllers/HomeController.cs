using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;

namespace CatAclysmeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly CatAclysmeContext _context;

        // Injection de CatAclysmeContext dans le contrôleur
        public HomeController(CatAclysmeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Action pour tester l'accès à la base de données
        public IActionResult TestDb()
        {
            // Exemple : compter le nombre de joueurs dans la base de données
            var playerCount = _context.Players.Count();
            var cardCount = _context.Cards.Count();

            // Afficher le nombre de joueurs sur une page ou dans la console
            return Content($"Nombre de joueurs dans la base de données : {playerCount} et nombre de cartes : {cardCount}" );
        }

        public async Task<IActionResult> Register(string name, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                return View(); // On renvoie la vue avec un message d'erreur si nécessaire
            }

            // Hash du mot de passe
            var passwordHasher = new PasswordHasher<Player>();
            var player = new Player { Name = name };
            player.Password = passwordHasher.HashPassword(player, password);

            // Ajouter l'utilisateur dans la base de données
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login"); // Redirige vers la page de connexion après l'inscription
        }

        public async Task<IActionResult> Login(string name, string password)
        {
            var player = _context.Players.SingleOrDefault(p => p.Name == name);

            if (player != null)
            {
                var passwordHasher = new PasswordHasher<Player>();
                var result = passwordHasher.VerifyHashedPassword(player, player.Password, password);

                if (result == PasswordVerificationResult.Success)
                {
                    // Connexion réussie, enregistrer la session utilisateur
                    HttpContext.Session.SetInt32("PlayerId", player.PlayerId);
                    return RedirectToAction("StartGame");
                }
            }

            return View(); // Si la connexion échoue, revenir à la page de connexion
        }

        public async Task<IActionResult> StartLocalGame(string player1Pseudo, string player2Pseudo)
        {
            // Vérifier et créer Joueur 1 si nécessaire
            var player1 = _context.Players.SingleOrDefault(p => p.Name == player1Pseudo);
            if (player1 == null)
            {
                player1 = new Player { Name = player1Pseudo };
                _context.Players.Add(player1);
                await _context.SaveChangesAsync();
            }

            // Vérifier et créer Joueur 2 si nécessaire
            var player2 = _context.Players.SingleOrDefault(p => p.Name == player2Pseudo);
            if (player2 == null)
            {
                player2 = new Player { Name = player2Pseudo };
                _context.Players.Add(player2);
                await _context.SaveChangesAsync();
            }

            // Créer une nouvelle partie
            var game = new Game
            {
                Player1HP = 300,
                Player2HP = 300,
                PlayerTurn = player1.PlayerId, // Commencer avec le premier joueur
                TurnCount = 0,
                GameStatus = 1, // Statut "en cours"
                PlayerId = player1.PlayerId,
                PlayerId_1 = player2.PlayerId
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            // Rediriger vers le tableau de jeu
            return RedirectToAction("GameBoard", new { gameId = game.GameId });
        }
        public async Task<IActionResult> EnterGame(string pseudo)
        {
            // Vérifier si le joueur existe déjà
            var player = _context.Players.SingleOrDefault(p => p.Name == pseudo);

            if (player == null)
            {
                // Si le joueur n'existe pas, on le crée
                player = new Player { Name = pseudo };
                _context.Players.Add(player);
                await _context.SaveChangesAsync();
            }

            // Enregistrer l'ID du joueur dans la session
            HttpContext.Session.SetInt32("PlayerId", player.PlayerId);

            // Rediriger vers la page de lancement de la partie ou le tableau de jeu
            return RedirectToAction("StartGame");
        }
    }
}
