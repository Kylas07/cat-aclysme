using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;
using System.Threading.Tasks;
using System.Linq;

namespace CatAclysmeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly CatAclysmeContext _context;

        public GameController(CatAclysmeContext context)
        {
            _context = context;
        }

        // POST : api/game/start
        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] GameStartRequest request)
        {
            // Vérifier les pseudos des joueurs
            if (string.IsNullOrEmpty(request.Player1Pseudo) || string.IsNullOrEmpty(request.Player2Pseudo))
            {
                return BadRequest(new { message = "Les pseudos des joueurs sont requis." });
            }

            // Créer ou récupérer Joueur 1
            var player1 = _context.Players.SingleOrDefault(p => p.Name == request.Player1Pseudo);
            if (player1 == null)
            {
                player1 = new Player { Name = request.Player1Pseudo };
                _context.Players.Add(player1);
                await _context.SaveChangesAsync();
            }

            // Créer ou récupérer Joueur 2
            var player2 = _context.Players.SingleOrDefault(p => p.Name == request.Player2Pseudo);
            if (player2 == null)
            {
                player2 = new Player { Name = request.Player2Pseudo };
                _context.Players.Add(player2);
                await _context.SaveChangesAsync();
            }

            // Créer une nouvelle partie
            var game = new Game
            {
                Player1HP = 100,
                Player2HP = 100,
                PlayerTurn = player1.PlayerId, // Commencer avec le premier joueur
                TurnCount = 1,
                GameStatus = 1, // Statut "en cours"
                PlayerId = player1.PlayerId,
                PlayerId_1 = player2.PlayerId,
                Player = player1,     // Initialiser le joueur 1 dans l'objet Game
                Player_1 = player2    // Initialiser le joueur 2 dans l'objet Game
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            // Retourner l'ID du jeu nouvellement créé
            return Ok(new { gameId = game.GameId });
        }
    }

    public class GameStartRequest
    {
        public required string Player1Pseudo { get; set; }
        public required string Player2Pseudo { get; set; }
    }
}
