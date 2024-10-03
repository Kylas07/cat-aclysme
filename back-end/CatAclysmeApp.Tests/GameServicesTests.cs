using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;
using back_end.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using back_end.Request;
using back_end.Response;

namespace back_end.Tests
{
    public class GameServiceTests
    {
        private readonly DbContextOptions<CatAclysmeContext> _options;

        public GameServiceTests()
        {
            // Configuration de la base de données InMemory pour les tests
            _options = new DbContextOptionsBuilder<CatAclysmeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task StartGame_WithValidPlayers_ReturnsGameStartResponse()
        {
            // Arrange : Créer un contexte InMemory et ajouter les joueurs sans spécifier les PlayerId
            using var context = new CatAclysmeContext(_options);

            var player1 = new Player
            {
                Name = "Player1",
                Password = "password123",
                Deck = new Deck { Name = "Deck1" }  // Ne spécifiez pas DeckId
            };

            var player2 = new Player
            {
                Name = "Player2",
                Password = "password456",
                Deck = new Deck { Name = "Deck2" }  // Ne spécifiez pas DeckId
            };

            context.Players.AddRange(player1, player2);
            await context.SaveChangesAsync();  // Entity Framework génère automatiquement les PlayerId

            var gameService = new GameService(context);

            var request = new GameStartRequest
            {
                Player1Pseudo = "Player1",
                Player2Pseudo = "Player2"
            };

            // Act : Appeler la méthode StartGame
            var result = await gameService.StartGame(request);

            // Assert : Vérifier le contenu de la réponse
            Assert.NotNull(result);
            Assert.NotNull(result.Game);
            Assert.Equal(100, result.Game.Player1HP);
            Assert.Equal(100, result.Game.Player2HP);
            Assert.Equal(8, result.Game.Board.Count); // 8 slots sur le plateau
            Assert.Equal(player1.PlayerId, result.Game.PlayerTurn); // Le joueur 1 commence
        }


        [Fact]
        public async Task StartGame_WithInvalidPlayers_ThrowsException()
        {
            // Arrange : Créer un contexte InMemory sans joueurs
            using var context = new CatAclysmeContext(_options);
            var gameService = new GameService(context);

            var request = new GameStartRequest
            {
                Player1Pseudo = "NonExistentPlayer1",
                Player2Pseudo = "NonExistentPlayer2"
            };

            // Act & Assert : Vérifier que la méthode lance une exception si les joueurs n'existent pas
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await gameService.StartGame(request)
            );
        }

        
    }
}
