using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockManager.Models;

namespace StockManager.Controllers
{

    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View("~/Views/Home/Index.cshtml");
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View("~/Views/Home/Index.cshtml");
            }

            
            if (await _userManager.IsInRoleAsync(user, "Jefe"))
                return RedirectToAction("Index", "Usuarios");
            else if (await _userManager.IsInRoleAsync(user, "Administrador"))
                return RedirectToAction("Index", "Productos");
            else
                return RedirectToAction("Index", "Movimientos");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
