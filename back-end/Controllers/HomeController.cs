using Microsoft.AspNetCore.Mvc;

namespace CatAclysmeApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}