using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockManager.Models;

namespace StockManager.Controllers
{
    //[Authorize]
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (!result.Succeeded)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View();
            }

            // Obtenemos el usuario y su rol
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            var rol = roles.FirstOrDefault();

            // Redirigimos según el rol
            //return rol switch
            //{
            //    "Jefe" => RedirectToAction("Index", "Usuarios"),
            //    "Administrador" => RedirectToAction("Index", "Productos"),
            //    "Empleado" => RedirectToAction("Index", "Movimientos"),
            //    _ => RedirectToAction("Index", "Home")
            //};
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
