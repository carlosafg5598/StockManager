using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockManager.Models;

namespace StockManager.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public PerfilController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model, IFormFile? FotoPerfil, IFormFile? FondoPantalla)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            user.NombreCompleto = model.NombreCompleto;
            user.Email = model.Email;
            user.FuentePreferida = model.FuentePreferida;
            user.TemaColor = model.TemaColor;
            user.PhoneNumber = model.PhoneNumber;


            // Guardar imagen si el usuario sube una nueva
            if (FotoPerfil != null && FotoPerfil.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "perfiles");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(FotoPerfil.FileName)}";
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await FotoPerfil.CopyToAsync(stream);
                }

                user.FotoPerfil = $"/uploads/perfiles/{fileName}";
            }

            if (FondoPantalla != null && FondoPantalla.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "fondos");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(FondoPantalla.FileName)}";
                var filePath = Path.Combine(uploads, fileName);

                
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await FondoPantalla.CopyToAsync(stream);

                user.FondoPantalla = $"/uploads/fondos/{fileName}";
            }

            await _userManager.UpdateAsync(user);

            TempData["Exito"] = "Perfil actualizado correctamente.";
            return RedirectToAction("Edit");
        }
        // --- CAMBIO DE CONTRASEÑA ---
        [HttpGet]
        public IActionResult CambiarContrasena()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(string contraseñaActual, string nuevaContraseña, string confirmarContraseña)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (nuevaContraseña != confirmarContraseña)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas nuevas no coinciden.");
                return View();
            }

            var result = await _userManager.ChangePasswordAsync(user, contraseñaActual, nuevaContraseña);

            if (result.Succeeded)
            {
                TempData["Exito"] = "Contraseña actualizada correctamente.";
                return RedirectToAction("Edit");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

    }
}
