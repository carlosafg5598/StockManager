using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockManager.Models;

namespace StockManager.Controllers
{
    [Authorize(Roles = "Jefe")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = _userManager.Users.ToList();
            var model = new List<(ApplicationUser Usuario, string Rol)>();

            foreach (var u in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(u);
                model.Add((u, roles.FirstOrDefault() ?? "Sin Rol"));
            }

            return View(model);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles
                .Where(r => r.Name == "Empleado" || r.Name == "Administrador")
                .Select(r => r.Name)
                .ToList();

            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string NombreCompleto, string email, string password, string rol, string PhoneNumber)
        {
            
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(rol))
            {
                ViewBag.Error = "Todos los campos obligatorios deben rellenarse.";
                ViewBag.Roles = _roleManager.Roles
                    .Where(r => r.Name == "Empleado" || r.Name == "Administrador")
                    .Select(r => r.Name)
                    .ToList();
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                NombreCompleto = string.IsNullOrWhiteSpace(NombreCompleto) ? email.Split('@')[0] : NombreCompleto,
                PhoneNumber = PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join(" ", result.Errors.Select(e => e.Description));
                ViewBag.Roles = _roleManager.Roles
                    .Where(r => r.Name == "Empleado" || r.Name == "Administrador")
                    .Select(r => r.Name)
                    .ToList();
                return View();
            }

            
            if (rol != "Empleado" && rol != "Administrador")
            {
                
                await _userManager.DeleteAsync(user);
                ViewBag.Error = "Rol no válido.";
                ViewBag.Roles = _roleManager.Roles
                    .Where(r => r.Name == "Empleado" || r.Name == "Administrador")
                    .Select(r => r.Name)
                    .ToList();
                return View();
            }

            await _userManager.AddToRoleAsync(user, rol);

            
            return RedirectToAction(nameof(Index));
        }


        
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction(nameof(Index));

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                
                return RedirectToAction(nameof(Index));
            }

            
            return RedirectToAction(nameof(Index));
        }



        
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            
            ViewBag.Roles = _roleManager.Roles
                .Where(r => r.Name == "Empleado" || r.Name == "Administrador")
                .Select(r => r.Name)
                .ToList();

            
            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            ViewBag.CurrentRole = currentRole;

            return View(user);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string email, string telefono, string rol)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(rol))
            {
                ViewBag.Error = "Email y rol son obligatorios.";
                return View(user);
            }


            user.Email = email;
            user.UserName = email;
            user.PhoneNumber = telefono;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
                return View(user);
            }

            
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, rol);

            return RedirectToAction("Index");
        }

    }
}
