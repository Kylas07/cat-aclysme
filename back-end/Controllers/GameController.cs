using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        // 1. Démarrage d'une partie
        // POST : api/game/start
        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] GameStartRequest request)
        {
            // Vérifier les pseudos des joueurs
            if (string.IsNullOrEmpty(request.Player1Pseudo) || string.IsNullOrEmpty(request.Player2Pseudo))
            {
                return BadRequest(new { message = "Les pseudos des joueurs sont requis." });
            }

            // Rechercher ou créer Joueur 1
            var player1 = _context.Players.SingleOrDefault(p => p.Name == request.Player1Pseudo);
            player1 ??= new Player 
            { 
                Name = request.Player1Pseudo, 
                Deck = new Deck 
                { 
                    Name = "Deck de " + request.Player1Pseudo,
                    Player = player1  // Initialisation temporaire (sera corrigée après)
                }
            };

            // Ajouter Joueur 1 s'il a été créé
            if (player1.PlayerId == 0)
            {
                _context.Players.Add(player1);
                await _context.SaveChangesAsync();  // Sauvegarder pour générer l'ID
                player1.Deck.Player = player1;  // Corriger la référence du deck après l'insertion
            }

            // Rechercher ou créer Joueur 2
            var player2 = _context.Players.SingleOrDefault(p => p.Name == request.Player2Pseudo);
            player2 ??= new Player 
            { 
                Name = request.Player2Pseudo, 
                Deck = new Deck 
                { 
                    Name = "Deck de " + request.Player2Pseudo,
                    Player = player2  // Initialisation temporaire (sera corrigée après)
                }
            };

            // Ajouter Joueur 2 s'il a été créé
            if (player2.PlayerId == 0)
            {
                _context.Players.Add(player2);
                await _context.SaveChangesAsync();  // Sauvegarder pour générer l'ID
                player2.Deck.Player = player2;  // Corriger la référence du deck après l'insertion
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
                Player = player1,
                Player_1 = player2
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            // Retourner l'ID du jeu nouvellement créé
            return Ok(new { gameId = game.GameId });
        }

        // 2. Gestion des tours
        // POST : api/game/draw-card
        [HttpPost("draw-card")]
        public async Task<IActionResult> DrawCard([FromBody] DrawCardRequest request)
        {
            // Récupérer la partie
            var game = await _context.Games
                .Include(g => g.Player)
                .Include(g => g.Player_1)
                .FirstOrDefaultAsync(g => g.GameId == request.GameId);

            if (game == null)
                return NotFound(new { message = "Partie non trouvée." });

            // Vérifier que c'est bien le tour du joueur
            if (game.PlayerTurn != request.PlayerId)
                return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });

            // Récupérer le joueur
            var player = await _context.Players
                .Include(p => p.Deck)
                .ThenInclude(d => d.Cards)
                .Include(p => p.Hand)  // Inclure la main du joueur
                .FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId);

            if (player == null || player.Deck.Cards.Count == 0)
                return BadRequest(new { message = "Le joueur n'a plus de cartes à piocher." });

            // Tirer une carte du deck
            var card = player.Deck.Cards.First();  // Choisit la première carte pour simplifier
            player.Deck.Cards.Remove(card);  // Retirer la carte du deck

            // Ajouter la carte à la main du joueur sous forme de PlayerHand
            var playerHand = new PlayerHand
            {
                PlayerId = player.PlayerId,
                Player = player,
                CardId = card.CardId,
                Card = card,
                Game = game  // Initialiser la propriété Game
            };

            player.Hand.Add(playerHand);  // Ajouter l'objet PlayerHand à la main du joueur

            await _context.SaveChangesAsync();

            return Ok(new { cardId = card.CardId, cardName = card.Name });
        }


        // POST : api/game/end-turn
        [HttpPost("end-turn")]
        public async Task<IActionResult> EndTurn([FromBody] EndTurnRequest request)
        {
            // Récupérer la partie
            var game = await _context.Games.FirstOrDefaultAsync(g => g.GameId == request.GameId);
            if (game == null)
                return NotFound(new { message = "Partie non trouvée." });

            // Vérifier que c'est bien le tour du joueur
            if (game.PlayerTurn != request.PlayerId)
                return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });

            // Passer au joueur suivant
            game.PlayerTurn = (game.PlayerTurn == game.PlayerId) ? game.PlayerId_1 : game.PlayerId;

            // Incrémenter le nombre de tours
            game.TurnCount++;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Tour terminé", nextPlayer = game.PlayerTurn });
        }

        // 3. Gestion des cartes (pose et attaque)
        // POST : api/game/play-card
        [HttpPost("play-card")]
        public async Task<IActionResult> PlayCard([FromBody] PlayCardRequest request)
        {
            // Récupérer la partie
            var game = await _context.Games.FirstOrDefaultAsync(g => g.GameId == request.GameId);
            if (game == null)
                return NotFound(new { message = "Partie non trouvée." });

            // Vérifier que c'est bien le tour du joueur
            if (game.PlayerTurn != request.PlayerId)
                return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });

            // Récupérer le joueur
            var player = await _context.Players
                .Include(p => p.Hand)
                .FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId);

            if (player == null)
                return NotFound(new { message = "Joueur non trouvé." });

            // Vérifier que la carte est dans la main du joueur
            var card = player.Hand.FirstOrDefault(c => c.CardId == request.CardId);
            if (card == null)
                return BadRequest(new { message = "Carte non trouvée dans la main du joueur." });

            // Le front-end gère maintenant la pose de la carte sur le plateau
            player.Hand.Remove(card);  // Retirer la carte de la main

            // Finir le tour (passe au joueur suivant)
            game.PlayerTurn = (game.PlayerTurn == game.PlayerId) ? game.PlayerId_1 : game.PlayerId;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Carte posée avec succès." });
        }

        // POST : api/game/attack
        [HttpPost("attack")]
        public async Task<IActionResult> Attack([FromBody] AttackRequest request)
        {
            // Récupérer la partie
            var game = await _context.Games.FirstOrDefaultAsync(g => g.GameId == request.GameId);
            if (game == null)
                return NotFound(new { message = "Partie non trouvée." });

            // Vérifier que c'est bien le tour du joueur
            if (game.PlayerTurn != request.PlayerId)
                return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });

            // Logique d'attaque à implémenter selon les règles de ton jeu (attaquer une carte ou l'adversaire)

            // Mettre à jour les HP des joueurs ou des cartes
            await _context.SaveChangesAsync();

            return Ok(new { message = "Attaque réussie." });
        }

        // 4. Fin de la partie
        // POST : api/game/end
        [HttpPost("end")]
        public async Task<IActionResult> EndGame([FromBody] EndGameRequest request)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.GameId == request.GameId);

            if (game == null)
                return NotFound(new { message = "Partie non trouvée." });

            // Marquer la partie comme terminée
            game.GameStatus = 0;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Partie terminée." });
        }
    }

    // Modèles de requêtes
    public class GameStartRequest
    {
        public required string Player1Pseudo { get; set; }
        public required string Player2Pseudo { get; set; }
    }

    public class DrawCardRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }

    public class PlayCardRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int CardId { get; set; }
    }

    public class AttackRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int BoardSlotId { get; set; }  // Emplacement de la carte qui attaque
    }

    public class EndTurnRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }

    public class EndGameRequest
    {
        public int GameId { get; set; }
    }
}
