using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;

namespace CatAclysmeApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly CatAclysmeContext _context;

        // Injection de CatAclysmeContext dans le contrôleur
        public HomeController(CatAclysmeContext context)
        {
            _context = context;
        }

        // Action pour tester l'accès à la base de données
        public IActionResult TestDb()
        {
            // Exemple : compter le nombre de joueurs dans la base de données
            var playerCount = _context.Players.Count();

            // Afficher le nombre de joueurs sur une page ou dans la console
            return Content($"Nombre de joueurs dans la base de données : {playerCount}");

            var cardCount = _context.Cards.Count();

            return Content($"Nombre de cartes dans la base de données : {cardCount}");
        }
    }
}