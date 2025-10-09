using Microsoft.AspNetCore.Mvc;
using StockManager.Models.Data;

namespace StockManager.Controllers
{
    public class UsuariosController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }
    }
}
