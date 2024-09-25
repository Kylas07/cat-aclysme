using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Helpers;

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
            if (!IsRequestValid(request, out string? errorMessage))
            {
                return BadRequest(errorMessage);
            }

            var player1 = await _context.Players.SingleOrDefaultAsync(p => p.Name == request.Player1Pseudo);
            var player2 = await _context.Players.SingleOrDefaultAsync(p => p.Name == request.Player2Pseudo);

            if (player1 == null || player2 == null)
            {
                return BadRequest("Les joueurs doivent être valides.");
            }

            // Initialiser les decks aléatoires pour chaque joueur
            var deck1 = await InitializeDeck(player1);
            var deck2 = await InitializeDeck(player2);

            // Créer la nouvelle partie en initialisant toutes les propriétés
            var game = new Game
            {
                Player1HP = 100,  // Points de vie initial pour le joueur 1
                Player2HP = 100,  // Points de vie initial pour le joueur 2
                PlayerTurn = player1.PlayerId,  // Le joueur 1 commence
                TurnCount = 1,  // Tour initial
                GameStatus = 1,  // Status en cours de la partie
                PlayerId = player1.PlayerId,  // ID du joueur 1
                PlayerId_1 = player2.PlayerId,  // ID du joueur 2
                Player = player1,  // Objet Player pour le joueur 1
                Player_1 = player2,  // Objet Player pour le joueur 2
                Player1Deck = deck1,  // Deck du joueur 1
                Player2Deck = deck2,  // Deck du joueur 2
                Player1Hand = GenerateStartingHand(deck1),  // Main de départ du joueur 1
                Player2Hand = GenerateStartingHand(deck2),  // Main de départ du joueur 2
                CardsOnBoard = new List<Card>()  // Plateau de cartes vide
            };

            // Sauvegarder la partie dans la base de données
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return Ok(new { gameId = game.GameId });
        }


        // POST : api/game/state
        [HttpGet("state")]
        public async Task<IActionResult> GetGameState(int gameId)
        {
            var game = await _context.Games
                .Include(g => g.Player)
                .Include(g => g.Player_1)
                .Include(g => g.Player.Hand)
                .Include(g => g.Player_1.Hand)
                .FirstOrDefaultAsync(g => g.GameId == gameId);

            if (game == null)
                return NotFound(new { message = "Partie non trouvée." });

            return Ok(new
            {
                gameId = game.GameId,
                player1HP = game.Player1HP,
                player2HP = game.Player2HP,
                playerTurn = game.PlayerTurn,
                player1Hand = game.Player.Hand.Select(h => new { h.Card.CardId, h.Card.Name, h.Card.Attack, h.Card.Health }),
                player2Hand = game.Player_1.Hand.Select(h => new { h.Card.CardId, h.Card.Name, h.Card.Attack, h.Card.Health }),
                player1DeckSize = game.Player.Deck.Cards.Count,
                player2DeckSize = game.Player_1.Deck.Cards.Count,
                cardsOnBoard = game.CardsOnBoard // Assurez-vous que cette collection existe dans votre modèle
            });
        }

        // Méthode pour initialiser un deck aléatoire de 30 cartes depuis la base de données pour un joueur
        private async Task<List<Card>> InitializeDeck(Player player)
        {
            // Récupérer toutes les cartes disponibles dans la base de données
            var allCards = await _context.Cards.ToListAsync();

            // Vérifier s'il y a suffisamment de cartes dans la base
            if (allCards.Count < 30)
            {
                throw new InvalidOperationException("Il n'y a pas assez de cartes dans la base de données pour créer un deck.");
            }

            // Sélectionner aléatoirement 30 cartes
            var random = new Random();
            var randomCards = allCards.OrderBy(c => random.Next()).Take(30).ToList();

            return randomCards; // Renvoie directement une List<Card>
        }


        // Méthode pour générer la main de départ (5 cartes) pour un joueur
        private List<Card> GenerateStartingHand(List<Card> cards)
        {
            if (cards.Count < 5)
            {
                throw new InvalidOperationException("Le deck n'a pas assez de cartes pour générer une main de départ.");
            }

            // Sélectionner les 5 premières cartes du deck
            var startingHand = cards.Take(5).ToList();

            // Retirer ces cartes du deck
            cards.RemoveRange(0, 5);

            return startingHand;
        }
        // Méthode de validation des pseudos
        private bool IsRequestValid(GameStartRequest request, out string? errorMessage)
        {
            if (string.IsNullOrEmpty(request.Player1Pseudo) || string.IsNullOrEmpty(request.Player2Pseudo))
            {
                errorMessage = "Les pseudos des joueurs sont requis.";
                return false;
            }

            if (request.Player1Pseudo == request.Player2Pseudo)
            {
                errorMessage = "Les pseudos des joueurs doivent être différents.";
                return false;
            }

            errorMessage = null;
            return true;
        }

        // 2. Gestion des tours
        // POST : api/game/draw-card[HttpPost("draw-card")]
        [HttpPost("draw-card")]
        public async Task<IActionResult> DrawCard([FromBody] DrawCardRequest request)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.GameId == request.GameId);

            if (game == null)
                return NotFound(new { message = "Partie non trouvée." });

            if (game.PlayerTurn != request.PlayerId)
                return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });

            // Sélectionner le deck du joueur en fonction de son ID
            var playerDeck = (game.PlayerId == request.PlayerId) ? game.Player1Deck : game.Player2Deck;

            if (playerDeck.Count == 0)
                return BadRequest(new { message = "Le joueur n'a plus de cartes à piocher." });

            var drawnCard = playerDeck.First();
            playerDeck.Remove(drawnCard);

            var playerHand = (game.PlayerId == request.PlayerId) ? game.Player1Hand : game.Player2Hand;
            playerHand.Add(drawnCard);

            await _context.SaveChangesAsync();

            return Ok(new { cardId = drawnCard.CardId, cardName = drawnCard.Name });
        }




        // POST : api/game/play-card
        [HttpPost("play-card")]
        public async Task<IActionResult> PlayCard([FromBody] PlayCardRequest request)
        {
            // Récupérer la partie en mémoire
            var game = await _context.Games
                .FirstOrDefaultAsync(g => g.GameId == request.GameId);

            if (game == null)
                return NotFound(new { message = "Partie non trouvée." });

            // Vérifier que c'est bien le tour du joueur
            if (game.PlayerTurn != request.PlayerId)
                return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });

            // Récupérer la carte dans la main du joueur
            var playerHand = (game.PlayerId == request.PlayerId) ? game.Player1Hand : game.Player2Hand;
            var cardToPlay = playerHand.FirstOrDefault(c => c.CardId == request.CardId);
            if (cardToPlay == null)
                return BadRequest(new { message = "Carte non trouvée dans la main du joueur." });

            // Poser la carte sur le plateau (ajouter à CardsOnBoard)
            game.CardsOnBoard.Add(cardToPlay);

            // Retirer la carte de la main du joueur
            playerHand.Remove(cardToPlay);

            // Tu peux envoyer une réponse avec les informations mises à jour
            return Ok(new { message = "Carte posée sur le plateau avec succès." });
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
