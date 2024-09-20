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

        [HttpGet]
        public IActionResult StartLocalGame()
        {
            return View(); // Cette vue affiche le formulaire de saisie
        }

        [HttpPost]
        public async Task<IActionResult> StartLocalGame(string player1Pseudo, string player2Pseudo)
        {
            // Ajout de logs pour vérifier les valeurs
            Console.WriteLine($"Player 1: {player1Pseudo}, Player 2: {player2Pseudo}");

            if (string.IsNullOrEmpty(player1Pseudo) || string.IsNullOrEmpty(player2Pseudo))
            {
                return BadRequest("Les pseudonymes des joueurs ne peuvent pas être vides.");
            }

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

            // Créer une nouvelle partie si aucune n'est en cours
            var game = new Game
            {
                Player1HP = 100,
                Player2HP = 100,
                PlayerTurn = player1.PlayerId, // Commencer avec le premier joueur
                TurnCount = 0,
                GameStatus = 1, // Statut "en cours"
                PlayerId = player1.PlayerId,
                PlayerId_1 = player2.PlayerId,
                Player = player1,     // Initialiser le joueur 1 dans l'objet Game
                Player_1 = player2    // Initialiser le joueur 2 dans l'objet Game
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            // Rediriger vers le tableau de jeu
            return RedirectToAction("GameBoard", new { gameId = game.GameId });
        }

        public IActionResult GameBoard(int gameId)
        {
            var game = _context.Games
                .Include(g => g.Player)      // Inclure les infos du Joueur 1
                .Include(g => g.Player_1)    // Inclure les infos du Joueur 2
                .FirstOrDefault(g => g.GameId == gameId);

            if (game == null)
            {
                return NotFound("Partie non trouvée.");
            }

            // Déterminer le pseudo du joueur dont c'est le tour
            string joueurTourPseudo = game.PlayerTurn == game.PlayerId ? game.Player.Name : game.Player_1.Name;

            // Passer les données à la vue avec ViewBag
            ViewBag.JoueurTourPseudo = joueurTourPseudo;

            return View(game);
        }
    }
}
