using Microsoft.AspNetCore.Mvc;
using StockManager.Models.Data;

namespace StockManager.Controllers
{
    public class ProductosController : Controller
    {

        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
