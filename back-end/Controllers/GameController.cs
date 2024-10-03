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
        [HttpPost("draw-card")]
        public async Task<IActionResult> DrawCard([FromBody] DrawCardRequest request)
        {
            try
            {
                var drawnCard = await _gameService.DrawCard(request);

                if (drawnCard == null)
                {
                    return BadRequest(new { success = false, message = "Aucune carte disponible à piocher." });
                }

                var remainingCards = await _gameService.GetRemainingCardsInDeck(request.GameId, request.PlayerId);

                return Ok(new { success = true, card = drawnCard, remainingCards });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Une erreur est survenue.", error = ex.Message });
            }
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


            // **Ajout dans GameCard pour suivre la carte jouée dans la partie**
            var gameCard = new GameCard
            {
                PlayerId = request.PlayerId,
                CardId = cardToPlay.CardId,
                GameId = request.GameId,
                CardPosition = request.BoardSlotIndex, // Position sur le plateau
                CurrentHealth = cardToPlay.Card.Health, // Initialiser avec la santé de la carte
                IsActive = true, // Indiquer que la carte est active
                Player = await _context.Players.FindAsync(request.PlayerId), // Récupérer le joueur depuis la BD
                Card = cardToPlay.Card, // Assigner la carte qui est jouée
                Game = game // Assigner la partie en cours
            };
            _context.GameCards.Add(gameCard);


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

            return Ok(new { message = "Carte posée avec succès.", gameCard });
        }

        // POST : api/game/attack
        [HttpPost("attack")]
        public async Task<IActionResult> AttackCard([FromBody] AttackRequest request)
        {
            Console.WriteLine($"Request received for attack: GameId={request.GameId}, PlayerId={request.PlayerId}, AttackerSlotIndex={request.BoardSlotId}, TargetSlotIndex={request.TargetBoardSlotId}");

            // Récupérer la partie et les cartes spécifiques à cette partie
            var game = await _context.Games
                .Include(g => g.Board)
                .ThenInclude(b => b.Card)
                .FirstOrDefaultAsync(d => d.GameId == request.GameId && d.PlayerId == request.PlayerId);

            if (game == null)
            {
                Console.WriteLine("Game not found");
                return NotFound(new { message = "Partie non trouvée." });
            }

            // Vérifier que c'est le tour du joueur
            if (game.PlayerTurn != request.PlayerId)
            {
                return BadRequest(new { message = "Ce n'est pas le tour de ce joueur." });
            }

            // Récupérer les cartes de la partie en cours (GameCard)
            var attackingGameCard = await _context.GameCards
                .FirstOrDefaultAsync(gc => gc.GameId == request.GameId
                    && gc.PlayerId == request.PlayerId
                    && gc.CardPosition == request.BoardSlotId);

            // Vérifier que la carte attaquante est toujours en jeu dans le GameDeck
            var attackingDeckCard = await _context.GameDecks
                .FirstOrDefaultAsync(gd => gd.GameId == request.GameId
                    && gd.CardId == attackingGameCard.CardId
                    && gd.CardState == CardState.OnBoard);

            // Récupérer la carte cible dans la partie (GameCard)
            var targetGameCard = await _context.GameCards
                .FirstOrDefaultAsync(gc => gc.GameId == request.GameId
                    && gc.PlayerId != request.PlayerId  // Opposant
                    && gc.CardPosition == request.TargetBoardSlotId);

            // Vérifier que la carte cible est toujours en jeu dans le GameDeck
            var targetDeckCard = await _context.GameDecks
                .FirstOrDefaultAsync(gd => gd.GameId == request.GameId
                    && gd.CardId == targetGameCard.CardId
                    && gd.CardState == CardState.OnBoard);
            if (targetGameCard != null)
            {
                // Simuler l'attaque
                targetGameCard.CurrentHealth -= attackingGameCard.Card.Attack;
                attackingGameCard.CurrentHealth -= targetGameCard.Card.Attack;

                Console.WriteLine($"{attackingGameCard.Card.Name} a attaqué {targetGameCard.Card.Name}. Santé du défenseur: {targetGameCard.CurrentHealth}, Santé de l'attaquant: {attackingGameCard.CurrentHealth}");

                // Si la carte est détruite, on met à jour son statut
                if (targetGameCard.CurrentHealth <= 0)
                {
                    targetDeckCard.CardState = CardState.Destroyed;
                    _context.GameCards.Remove(targetGameCard);
                }

                if (attackingGameCard.CurrentHealth <= 0)
                {
                    attackingDeckCard.CardState = CardState.Destroyed;
                    _context.GameCards.Remove(attackingGameCard);
                }

                // Sauvegarder les changements spécifiques à la partie
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    attackingCard = new 
                    {
                        Health = attackingGameCard.CurrentHealth,
                        State = attackingDeckCard.CardState
                    },

                    defendingCard = new
                    {
                        Health = targetGameCard.CurrentHealth,
                        State = targetDeckCard.CardState
                    },
                    message = targetGameCard.CurrentHealth <= 0
                        ? $"{attackingGameCard.Card.Name} a détruit {targetGameCard.Card.Name}."
                        : "Attaque effectuée."
                });
            }
            else
            {
                // Gérer les cas où la cible est directement le joueur (pas une carte)
                if (request.PlayerId == game.PlayerId)
                {
                    Console.WriteLine($"{attackingGameCard.Card.Name} inflige {attackingGameCard.Card.Attack} points de dégâts au joueur 2.");
                    return Ok(new
                    {
                        attackingCard = new { Health = attackingGameCard.CurrentHealth },
                        message = $"Le joueur 2 a reçu {attackingGameCard.Card.Attack} points de dégâts."
                    });
                }
                else
                {
                    Console.WriteLine($"{attackingGameCard.Card.Name} inflige {attackingGameCard.Card.Attack} points de dégâts au joueur 1.");
                    return Ok(new
                    {
                        attackingCard = new { Health = attackingGameCard.CurrentHealth },
                        message = $"Le joueur 1 a reçu {attackingGameCard.Card.Attack} points de dégâts."
                    });
                }
            }
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
