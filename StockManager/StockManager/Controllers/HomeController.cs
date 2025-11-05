using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Models;
using StockManager.Models.Data;
using System.Diagnostics;

namespace StockManager.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }

            return RedirectToAction("Index", "Login");
        }

        
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var rol = roles.FirstOrDefault() ?? "Sin rol";

            ViewBag.Nombre = user.NombreCompleto ?? user.UserName;
            ViewBag.Rol = rol;

            var ultimosMovimientos = await _context.MovimientosStock
                .Include(m => m.Producto)      
                .OrderByDescending(m => m.FechaMovimiento)
                .Take(3)
                .ToListAsync();

            return View(ultimosMovimientos);


            
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
