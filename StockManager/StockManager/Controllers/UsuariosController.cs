using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Models;
using StockManager.Models.Data;

namespace StockManager.Controllers
{
    public class UsuariosController : Controller
    {
        //TOD RESOLVER DUDAS
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

     

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        // POST: Usuarios/Create


        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public IActionResult LoginView()
        {
            return View();
        }

        // POST: Usuarios/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string EmailUsuario, string PasswordHashUsuario)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.EmailUsuario == EmailUsuario && u.PasswordHashUsuario == PasswordHashUsuario);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View("LoginView");
            }

            // Redirige según el rol
            if (usuario.RolUsuario == "Administrador")
                return RedirectToAction("Index", "Productos");
            else
                return RedirectToAction("Index", "Movimientos");
        }





    }
}
