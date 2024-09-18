using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CatAclysmeApp.Controllers
{
    public class PlayerController : Controller
    {
        private readonly CatAclysmeContext _context;

        public PlayerController(CatAclysmeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var players = await _context.Players.ToListAsync();
            return Ok(players); // Si vous renvoyez en JSON
        }

        public async Task<IActionResult> Details(int id)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player); // Ou une vue si nécessaire
        }
    }

        public class TestController : Controller
    {
        private readonly CatAclysmeContext _context;

        public TestController(CatAclysmeContext context)
        {
            _context = context;
        }

        // Action pour tester l'accès à la base de données
        [HttpGet("/test-db")]
        public IActionResult TestDatabaseConnection()
        {
            try
            {
                // Essaie de créer la base de données si elle n'existe pas
                _context.Database.EnsureCreated();
                return Ok("Connexion à la base de données réussie et base de données disponible.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur de connexion à la base de données : {ex.Message}");
            }
        }
    }
}