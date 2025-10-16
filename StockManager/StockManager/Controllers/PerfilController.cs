using Microsoft.AspNetCore.Mvc;

namespace StockManager.Controllers
{
    public class PerfilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
