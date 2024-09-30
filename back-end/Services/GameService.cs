using back_end.Enums;
using back_end.Models;
using back_end.Request;
using back_end.Response;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
    public class GameService
    {
        private readonly CatAclysmeContext _context;

        public GameService(CatAclysmeContext context)
        {
            _context = context;
        }

        public async Task<GameStartResponse> StartGame([FromBody] GameStartRequest request)
        {
            // Récupérer les joueurs avec leurs decks
            var (player1, player2) = await GetPlayersAsync(request.Player1Pseudo, request.Player2Pseudo);

            Console.WriteLine("Player 1: " + player1.Name);
            Console.WriteLine("Player 2: " + player2.Name);

            // Créer une nouvelle partie
            var game = new Game
            {
                Player1HP = 100,
                Player2HP = 100,
                PlayerTurn = player1.PlayerId, // Joueur 1 commence
                TurnCount = 1,
                GameStatus = 1,
                PlayerId = player1.PlayerId,
                PlayerId_1 = player2.PlayerId,
                Player = player1, // Initialisation de Player
                Player_1 = player2 // Initialisation de Player_1
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            // Initialiser les decks des joueurs
            var (player1GameDeck, player2GameDeck) = await InitializeDeck(player1, player2, game);

            return new GameStartResponse
            {
                Game = game,
                Player1GameDeck = player1GameDeck,
                Player2GameDeck = player2GameDeck,
            };
        }


        public async Task<(List<GameDeck>, List<GameDeck>)> InitializeDeck(Player player1, Player player2, Game game)
        {

            Console.WriteLine($"Deck player 1: {player1.Name}, player 2: {player2.Name}, game ID: {game.GameId}");
            if (player1.Deck == null)
            {
                Console.WriteLine($"Le joueur {player1.Name} n'a pas de deck attribué.");
            }
            Console.WriteLine($"Deck du joueur 1 a pour id: {player1.Deck.DeckId}");
            var player1Deck = ShuffleDeck(await GetBuildCardsAsync(player1.Deck.DeckId));
            var player2Deck = ShuffleDeck(await GetBuildCardsAsync(player2.Deck.DeckId));

            var gameDeckPlayer1 = CreateGameDecks(player1Deck, player1, game);
            var gameDeckPlayer2 = CreateGameDecks(player2Deck, player2, game);

            _context.GameDecks.AddRange(gameDeckPlayer1);
            _context.GameDecks.AddRange(gameDeckPlayer2);
            await _context.SaveChangesAsync();

            return (gameDeckPlayer1, gameDeckPlayer2);
        } 

        public async Task<(Player, Player)> GetPlayersAsync(string player1Pseudo, string player2Pseudo)
        {
            // Récupérer les joueurs avec leurs decks
            var player1 = await _context.Players
                .Include(p => p.Deck)
                .FirstOrDefaultAsync(p => p.Name == player1Pseudo);

            var player2 = await _context.Players
                .Include(p => p.Deck)
                .FirstOrDefaultAsync(p => p.Name == player2Pseudo);

            if (player1 == null || player2 == null) {
                throw new ArgumentException("Les joueurs doivent être valides.");
            }
            
            return (player1, player2);
        }

        private List<GameDeck> CreateGameDecks(List<Card> shuffledDeck, Player player, Game game)
        {
            return shuffledDeck.Select((card, index) => new GameDeck
            {
                Player = player,
                PlayerId = player.PlayerId,
                Game = game,
                GameId = game.GameId,
                Card = card,
                CardId = card.CardId,
                CardOrder = index,
                CardState = index < 5 ? CardState.InHand : CardState.InDeck
            }).ToList();
        }

        public async Task<List<Card>> GetBuildCardsAsync(int deckId)
        {
            return await _context.Builds
                .Where(x => x.DeckId == deckId)
                .Select(x => x.Card)
                .ToListAsync();
        }

        public List<Card> ShuffleDeck(List<Card> cards)
        {
            var random = new Random();
            return cards.OrderBy(_ => random.Next()).ToList();
        }

        //récupérer les decks dans la partie de chaque joueur
        public async Task<List<GameDeck>> GetGameDecksByPlayerIdAsync(int playerId)
        {
            return await _context.GameDecks
                .Include(g => g.Card) // Inclure les informations sur les cartes
                .Where(g => g.PlayerId == playerId)
                .ToListAsync();
        }
    }
}
