using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Models;
using StockManager.Models.Data;

namespace StockManager.Controllers
{
    //[Authorize(Roles = "Administrador,Empleado,Jefe")]
    public class MovimientosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MovimientosController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var movimientos = _context.MovimientosStock
                                      .Include(m => m.Producto)
                                      .Include(m => m.Usuario)
                                      .OrderByDescending(m => m.FechaMovimiento)
                                      .ToList();
            return View(movimientos);
        }

        //[Authorize(Roles = "Jefe,Empleado")]
        public IActionResult Create()
        {

            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["Error"] = "❌ No tienes permisos para editar movimientos.";
                return RedirectToAction("Index"); // Volvemos a la vista de movimientos
            }
            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Jefe,Empleado")]
        public async Task<IActionResult> Create(MovimientoStock movimiento)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["Error"] = "❌ No tienes permisos para editar movimientos.";
                return RedirectToAction("Index"); // Volvemos a la vista de movimientos
            }
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var producto = _context.Productos.Find(movimiento.IdProducto);

                if (movimiento.TipoMovimiento == "Salida" && movimiento.Cantidad > producto.StockActual)
                {
                    ModelState.AddModelError("Cantidad", $"No hay suficiente stock disponible. Stock actual: {producto.StockActual}");
                }
                else
                {
                    movimiento.FechaMovimiento = DateTime.Now;
                    movimiento.UsuarioId = user?.Id; 

                    _context.MovimientosStock.Add(movimiento);

                    if (movimiento.TipoMovimiento == "Entrada")
                        producto.StockActual += movimiento.Cantidad;
                    else
                        producto.StockActual -= movimiento.Cantidad;

                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
            return View(movimiento);
        }

        // EDITAR MOVIMIENTO
        //[Authorize(Roles = "Jefe,Empleado")]
        public IActionResult Edit(int id)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["Error"] = "❌ No tienes permisos para editar movimientos.";
                return RedirectToAction("Index"); // Volvemos a la vista de movimientos
            }
            var movimiento = _context.MovimientosStock.Find(id);
            if (movimiento == null) return NotFound();

            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
            return View(movimiento);
        }

        [HttpPost]
        //[Authorize(Roles = "Jefe,Empleado")]
        public IActionResult Edit(MovimientoStock movimiento)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["Error"] = "❌ No tienes permisos para editar movimientos.";
                return RedirectToAction("Index"); // Volvemos a la vista de movimientos
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
                return View(movimiento);
            }

            _context.MovimientosStock.Update(movimiento);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ELIMINAR MOVIMIENTO
        //[Authorize(Roles = "Jefe,Empleado")]
        public IActionResult Delete(int id)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["Error"] = "❌ No tienes permisos para editar movimientos.";
                return RedirectToAction("Index"); // Volvemos a la vista de movimientos
            }
            var movimiento = _context.MovimientosStock
                                     .Include(m => m.Producto)
                                     .Include(m => m.Usuario)
                                     .FirstOrDefault(m => m.IdMovimiento == id);
            if (movimiento == null) return NotFound();

            return View(movimiento);
        }

        [HttpPost, ActionName("Delete")]
        //[Authorize(Roles = "Jefe,Empleado")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["Error"] = "❌ No tienes permisos para editar movimientos.";
                return RedirectToAction("Index"); // Volvemos a la vista de movimientos
            }
            var movimiento = _context.MovimientosStock.Find(id);
            if (movimiento == null) return NotFound();

            _context.MovimientosStock.Remove(movimiento);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
