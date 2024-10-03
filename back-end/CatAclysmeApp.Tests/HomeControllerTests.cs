using Xunit;
using Moq; // Utilisé pour créer des mocks pour simuler des dépendances
using Microsoft.Extensions.Logging; // Permet de simuler le logger injecté
using CatAclysmeApp.Controllers;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;
using Microsoft.AspNetCore.Mvc; // Nécessaire pour tester les réponses d'API
using Microsoft.EntityFrameworkCore; // Utilisé pour la base de données en mémoire
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CatAclysmeApp.Tests
{
    // Classe de test pour HomeController
    public class HomeControllerTests
    {
        // Options pour utiliser une base de données InMemory pour les tests
        private readonly DbContextOptions<CatAclysmeContext> _options;
        // Mock pour logger
        private readonly ILogger<HomeController> _logger;

        // Constructeur du test pour initialiser les dépendances
        public HomeControllerTests()
        {
            // Initialisation d'une base de données InMemory avec le nom 'TestDatabase'
            _options = new DbContextOptionsBuilder<CatAclysmeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            
            // Création d'un Mock pour le logger, utilisé dans HomeController
            var loggerMock = new Mock<ILogger<HomeController>>();
            _logger = loggerMock.Object; // Injection du mock dans les tests
        }

        // Test pour vérifier que l'inscription échoue si les données sont manquantes
        [Fact]
        public async Task Register_WithMissingData_ReturnsBadRequest()
        {
            using var context = new CatAclysmeContext(_options); // Création du contexte InMemory
            var controller = new HomeController(context, _logger); // Création du contrôleur avec dépendances injectées

            // Arrange : Préparation des données de requête avec un nom vide
            var request = new HomeController.RegisterRequest
            {
                PlayerName = "",  // Nom manquant
                Password = "password123" // Mot de passe fourni
            };

            // Act : Appel de la méthode Register du contrôleur
            var result = await controller.Register(request);

            // Assert : Vérification que la réponse est un BadRequest avec le message correct
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Le nom et le mot de passe sont requis.", badRequestResult.Value);
        }

        // Test pour vérifier que l'inscription échoue si le joueur existe déjà
        [Fact]
        public async Task Register_WithExistingPlayer_ReturnsBadRequest()
        {
            // Arrange
            using var context = new CatAclysmeContext(_options);

            // Ajouter un joueur existant dans la base de données
            var existingPlayer = new Player
            {
                Name = "ExistingPlayer",
                Password = "hashedpassword",
                Deck = new Deck { Name = "ExistingDeck" }
            };
            
            context.Players.Add(existingPlayer);
            await context.SaveChangesAsync();  // Entity Framework génère automatiquement les PlayerId

            var controller = new HomeController(context, _logger);

            var request = new HomeController.RegisterRequest
            {
                PlayerName = "ExistingPlayer",  // Même nom que le joueur existant
                Password = "password123"
            };

            // Act
            var result = await controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Le nom d'utilisateur est déjà pris.", badRequestResult.Value);
        }


        // Test pour vérifier que l'inscription réussit avec des données valides
        [Fact]
        public async Task Register_WithValidData_ReturnsOk()
        {
            using var context = new CatAclysmeContext(_options);
            var controller = new HomeController(context, _logger);

            // Arrange : Préparation d'une requête avec des données valides
            var request = new HomeController.RegisterRequest
            {
                PlayerName = "NewPlayer", // Un nom qui n'existe pas encore
                Password = "password123"
            };

            // Act : Appel de la méthode Register
            var result = await controller.Register(request);

            // Assert : Vérification que la réponse est Ok avec le message correct
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Compte créé avec succès.", okResult.Value);
        }

        // Test pour vérifier que le ping fonctionne correctement
        [Fact]
        public void Ping_ReturnsOk()
        {
            using var context = new CatAclysmeContext(_options);
            var controller = new HomeController(context, _logger);

            // Act : Appel de la méthode Ping
            var result = controller.Ping();

            // Assert : Vérification que la réponse est Ok et que le message est correct
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value as dynamic;
            Assert.Equal("API is running", value.message);
        }
    }
}
