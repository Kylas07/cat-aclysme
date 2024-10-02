using back_end.Request;
using back_end.Response;
using back_end.Services;
using CatAclysmeApp.Data;
using CatAclysmeApp.Helpers;
using CatAclysmeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back_end.Enums;

namespace CatAclysmeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly CatAclysmeContext _context;
        private readonly GameService _gameService;

        public GameController(CatAclysmeContext context, GameService gameService)
        {
            _context = context;
            _gameService = gameService;
        }

        // 1. Démarrage d'une partie
        // POST : api/game/start
        [ProducesResponseType(typeof(GameStartResponse), 200)]
        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] GameStartRequest request)
        {
            return Ok(await _gameService.StartGame(request));
        }


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

            // Marquer errorMessage comme null en utilisant un nullable type
            errorMessage = null;
            return true;
        }


        private async Task<Player> FindOrCreatePlayerAsync(string playerName)
        {
            // Rechercher un joueur dans la base de données
            var player = await _context.Players.SingleOrDefaultAsync(p => p.Name == playerName);

            string tempPassword = PasswordHasher.HashPassword("DefaultTemporaryPassword");

            // Si le joueur n'existe pas, le créer avec un Deck
            if (player == null)
            {
                player = new Player
                {
                    Name = playerName,
                    Password = tempPassword,
                    Deck = new Deck
                    {
                        Name = $"Deck de {playerName}",
                        // Cards = new List<Card>()  // Initialiser une liste de cartes vide
                    }
                };

                _context.Players.Add(player);
                await _context.SaveChangesAsync();  // Sauvegarder pour générer l'ID du joueur

                // Assigner le joueur au Deck après avoir obtenu l'ID
                player.Deck.Player = player;  // Corriger la référence circulaire
                await _context.SaveChangesAsync();  // Sauvegarder les changements
            }

            return player;
        }
        
        [HttpGet("deck/{gameId}/{playerId}")]
        public async Task<IActionResult> GetGameDeck(int gameId, int playerId)
        {
            var gameDecks = await _context.GameDecks
                .Where(d => d.GameId == gameId && d.PlayerId == playerId)
                .Include(d => d.Card) // Inclure les informations de carte
                .ToListAsync();

            if (gameDecks == null || !gameDecks.Any())
            {
                return NotFound(new { message = "pas de deck associé" });
            }

            return Ok(new { values = gameDecks });
        }
        // 2. Gestion des tours
        // POST : api/game/draw-card
        // [HttpPost("draw-card")]
        // public async Task<IActionResult> DrawCard([FromBody] DrawCardRequest request)
        // {
        //     // Récupérer la partie
        //     var game = await _context.Games
        //         .Include(g => g.Player)
        //         .Include(g => g.Player_1)
        //         .FirstOrDefaultAsync(g => g.GameId == request.GameId);

        //     if (game == null)
        //         return NotFound(new { message = "Partie non trouvée." });

        //     // Vérifier que c'est bien le tour du joueur
        //     if (game.PlayerTurn != request.PlayerId)
        //         return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });

        //     // Récupérer le joueur
        //     var player = await _context.Players
        //         .Include(p => p.Deck)
        //         .ThenInclude(d => d.Cards)
        //         .Include(p => p.Hand)  // Inclure la main du joueur
        //         .FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId);

        //     if (player == null || player.Deck.Cards.Count == 0)
        //         return BadRequest(new { message = "Le joueur n'a plus de cartes à piocher." });

        //     // Tirer une carte du deck
        //     var card = player.Deck.Cards.First();  // Choisit la première carte pour simplifier
        //     player.Deck.Cards.Remove(card);  // Retirer la carte du deck

        //     // Ajouter la carte à la main du joueur sous forme de PlayerHand
        //     var playerHand = new PlayerHand
        //     {
        //         PlayerId = player.PlayerId,
        //         Player = player,
        //         CardId = card.CardId,
        //         Card = card,
        //         Game = game  // Initialiser la propriété Game
        //     };

        //     player.Hand.Add(playerHand);  // Ajouter l'objet PlayerHand à la main du joueur

        //     await _context.SaveChangesAsync();

        //     return Ok(new { cardId = card.CardId, cardName = card.Name });
        // }


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

            // Réinitialiser l'état de jeu (le joueur suivant pourra jouer une carte)
            game.HasPlayedCardThisTurn = false;

            // Incrémenter le nombre de tours
            game.TurnCount++;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Tour terminé", nextPlayer = game.PlayerTurn });
        }

        // POST : api/game/play-card
        [HttpPost("play-card")]
        public async Task<IActionResult> PlayCard([FromBody] PlayCardRequest request)
        {
            // Récupérer la partie avec le plateau
            var game = await _context.Games
                .Include(g => g.Board) // Inclure le plateau
                .FirstOrDefaultAsync(g => g.GameId == request.GameId);

            if (game == null)
                return NotFound(new { message = "Partie non trouvée." });

            // Vérifier que c'est bien le tour du joueur
            if (game.PlayerTurn != request.PlayerId)
                return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });

            // Vérifier si le joueur a déjà joué une carte ce tour
            if (game.HasPlayedCardThisTurn)
                return BadRequest(new { message = "Vous ne pouvez jouer qu'une seule carte par tour." });

            // Récupérer les cartes du joueur dans cette partie (main et deck)
            var playerGameDecks = await _context.GameDecks
                .Where(gd => gd.PlayerId == request.PlayerId && gd.GameId == request.GameId)
                .Include(gd => gd.Card) // Inclure les détails des cartes
                .ToListAsync();

            if (playerGameDecks == null || !playerGameDecks.Any())
                return NotFound(new { message = "Le joueur n'a pas de deck associé à cette partie." });

            // Filtrer pour obtenir les cartes qui sont dans la main du joueur
            var playerHand = playerGameDecks.Where(gd => gd.CardState == CardState.InHand).ToList();

            // Vérifier que la carte est bien dans la main du joueur
            var cardToPlay = playerHand.FirstOrDefault(c => c.CardId == request.CardId);

            if (cardToPlay == null)
                return BadRequest(new { message = "Carte non trouvée dans la main du joueur." });

            // Valider l'index du slot (doit être entre 0 et 7 pour un plateau de 8 slots)
            if (request.BoardSlotIndex < 0 || request.BoardSlotIndex >= game.Board.Count)
            {
                return BadRequest(new { message = "Index de l'emplacement invalide." });
            }

            // Récupérer l'emplacement du plateau où la carte doit être jouée
            var boardSlot = game.Board.FirstOrDefault(slot => slot.Index == request.BoardSlotIndex);
            if (boardSlot == null || boardSlot.Card != null)
            {
                return BadRequest(new { message = "L'emplacement est déjà occupé ou invalide." });
            }

            // Assigner la carte à l'emplacement du plateau
            boardSlot.Card = cardToPlay.Card;
            cardToPlay.CardState = CardState.OnBoard; // Mettre à jour l'état de la carte

            // Marquer que le joueur a joué une carte ce tour
            game.HasPlayedCardThisTurn = true;

            // Sauvegarder les changements dans la base de données
            await _context.SaveChangesAsync();

            // Journal pour afficher l'état du plateau après la mise à jour
            Console.WriteLine($"État du plateau après la mise à jour :");
            foreach (var slot in game.Board)
            {
                Console.WriteLine($"Emplacement {slot.Index}: {(slot.Card != null ? slot.Card.Name : "Vide")}");
            }

            return Ok(new { message = "Carte posée avec succès." });
        }

        // POST : api/game/attack
        [HttpPost("attack")]
        public async Task<IActionResult> AttackCard([FromBody] AttackRequest request)
        {
            // Log details for debugging
            Console.WriteLine($"Request received for attack: GameId={request.GameId}, PlayerId={request.PlayerId}, BoardSlotId={request.BoardSlotId}, TargetBoardSlotId={request.TargetBoardSlotId}");

            // Retrieve the game and the board
            var game = await _context.Games
                .Include(g => g.Board) // Include the board
                .FirstOrDefaultAsync(g => g.GameId == request.GameId);

            if (game == null)
            {
                Console.WriteLine("Game not found");
                return NotFound(new { message = "Partie non trouvée." });
            }

            // Log board state
            foreach (var slot in game.Board)
            {
                Console.WriteLine($"Slot {slot.BoardSlotId}: {(slot.Card != null ? slot.Card.Name : "Empty")}");
            }

            // Ensure it's the player's turn
            if (game.PlayerTurn != request.PlayerId)
            {
                return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });
            }

            // Get the attacking card slot
            var attackingSlot = game.Board.FirstOrDefault(slot => slot.BoardSlotId == request.BoardSlotId);
            if (attackingSlot == null || attackingSlot.Card == null)
            {
                Console.WriteLine("Attacking card not found");
                return BadRequest(new { message = "Carte attaquante introuvable." });
            }

            var attackingCard = attackingSlot.Card;

            // Get the target card slot
            var targetSlot = game.Board.FirstOrDefault(slot => slot.BoardSlotId == request.TargetBoardSlotId);
            if (targetSlot != null && targetSlot.Card != null)
            {
                var defendingCard = targetSlot.Card;

                // Battle logic
                defendingCard.Health -= attackingCard.Attack;

                if (defendingCard.Health <= 0)
                {
                    targetSlot.Card = null; // Remove defending card if destroyed
                }

                attackingCard.Health -= defendingCard.Attack;
                if (attackingCard.Health <= 0)
                {
                    attackingSlot.Card = null; // Remove attacking card if destroyed
                }
            }
            else
            {
                // No card in front, deal damage to the opponent
                if (request.PlayerId == game.PlayerId)
                {
                    game.Player2HP -= attackingCard.Attack;
                }
                else
                {
                    game.Player1HP -= attackingCard.Attack;
                }
            }

            // Save changes
            await _context.SaveChangesAsync();
            return Ok(new { message = "Attaque effectuée." });
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
}
