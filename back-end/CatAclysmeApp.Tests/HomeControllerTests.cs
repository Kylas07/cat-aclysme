using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using CatAclysmeApp.Controllers;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using back_end.Request; // Ajout de l'import correct pour RegisterRequest et LoginRequest
using Microsoft.Extensions.Caching.Memory; // For IMemoryCache

namespace CatAclysmeApp.Tests
{
    public class HomeControllerTests
    {
        private readonly DbContextOptions<CatAclysmeContext> _options;
        private readonly ILogger<HomeController> _logger;
        private readonly Mock<IMemoryCache> _memoryCacheMock;

        public HomeControllerTests()
        {
            _options = new DbContextOptionsBuilder<CatAclysmeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var loggerMock = new Mock<ILogger<HomeController>>();
            _logger = loggerMock.Object;

            // Mock for IMemoryCache
            _memoryCacheMock = new Mock<IMemoryCache>();
        }

        [Fact]
        public async Task Register_WithMissingData_ReturnsBadRequest()
        {
            using var context = new CatAclysmeContext(_options);
            var controller = new HomeController(context, _logger, _memoryCacheMock.Object);

            // Utilisation de RegisterRequest depuis le namespace back_end.Request
            var request = new RegisterRequest
            {
                PlayerName = "",  
                Password = "password123"
            };

            var result = await controller.Register(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Le nom et le mot de passe sont requis.", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_WithExistingPlayer_ReturnsBadRequest()
        {
            using var context = new CatAclysmeContext(_options);

            var existingPlayer = new Player
            {
                Name = "ExistingPlayer",
                Password = "hashedpassword",
                Deck = new Deck { Name = "ExistingDeck" }
            };
            
            context.Players.Add(existingPlayer);
            await context.SaveChangesAsync();

            var controller = new HomeController(context, _logger, _memoryCacheMock.Object);

            var request = new RegisterRequest
            {
                PlayerName = "ExistingPlayer", 
                Password = "password123"
            };

            var result = await controller.Register(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Le nom d'utilisateur est déjà pris.", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsOk()
        {
            using var context = new CatAclysmeContext(_options);
            var controller = new HomeController(context, _logger, _memoryCacheMock.Object);

            var request = new RegisterRequest
            {
                PlayerName = "NewPlayer",
                Password = "password123"
            };

            var result = await controller.Register(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Compte créé avec succès.", okResult.Value);
        }

        [Fact]
        public void Ping_ReturnsOk()
        {
            using var context = new CatAclysmeContext(_options);
            var controller = new HomeController(context, _logger, _memoryCacheMock.Object);

            var result = controller.Ping();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value as dynamic;
            Assert.Equal("API is running", value.message);
        }
    }
}
