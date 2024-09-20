using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using System.Linq;

namespace CatAclysmeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly CatAclysmeContext _context;
        private readonly ILogger<HomeController> _logger;

        // Injection de CatAclysmeContext dans le contrôleur
        public HomeController(CatAclysmeContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Expose un endpoint de test pour vérifier si l'API fonctionne
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { message = "API is running" });
        }

        // Action pour tester l'accès à la base de données
        [HttpGet("test-db")]
        public IActionResult TestDb()
        {
            // Exemple : compter le nombre de joueurs dans la base de données
            var playerCount = _context.Players.Count();
            var cardCount = _context.Cards.Count();

            // Retourner le nombre de joueurs et de cartes sous forme de JSON
            return Ok(new { playerCount, cardCount });
        }
    }
}
