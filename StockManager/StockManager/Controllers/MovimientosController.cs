using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Models;
using StockManager.Models.Data;

namespace StockManager.Controllers
{
    [Authorize(Roles = "Administrador,Empleado,Jefe")]
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

        
        public IActionResult Create()
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["ErrorMovimientos"] = "No tienes permisos para crear movimientos.";
                return RedirectToAction("Index");
            }

            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> Create(MovimientoStock movimiento)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["ErrorMovimientos"] = "No tienes permisos para crear movimientos.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var producto = _context.Productos.Find(movimiento.IdProducto);
                if (producto == null) return NotFound();

                
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

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
            return View(movimiento);
        }

        // EDITAR MOVIMIENTO
        
        public IActionResult Edit(int id)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["ErrorMovimientos"] = "No tienes permisos para editar movimientos.";
                return RedirectToAction("Index");
            }

            var movimiento = _context.MovimientosStock.Find(id);
            if (movimiento == null) return NotFound();

            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
            return View(movimiento);
        }

        [HttpPost]
        
        public async Task<IActionResult> Edit(MovimientoStock movimiento)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["ErrorMovimientos"] = "No tienes permisos para editar movimientos.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
                return View(movimiento);
            }

            
            var original = await _context.MovimientosStock
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdMovimiento == movimiento.IdMovimiento);
            if (original == null) return NotFound();

            
            var productoOld = await _context.Productos.FindAsync(original.IdProducto);
            if (productoOld == null) return NotFound();

            if (original.TipoMovimiento == "Entrada")
                productoOld.StockActual -= original.Cantidad;
            else if (original.TipoMovimiento == "Salida")
                productoOld.StockActual += original.Cantidad;

            
            var productoNew = await _context.Productos.FindAsync(movimiento.IdProducto);
            if (productoNew == null) return NotFound();

            if (movimiento.TipoMovimiento == "Entrada")
                productoNew.StockActual += movimiento.Cantidad;
            else if (movimiento.TipoMovimiento == "Salida")
                productoNew.StockActual -= movimiento.Cantidad;

            
            _context.MovimientosStock.Update(movimiento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ELIMINAR MOVIMIENTO
        
        public IActionResult Delete(int id)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["ErrorMovimientos"] = "No tienes permisos para eliminar movimientos.";
                return RedirectToAction("Index");
            }

            var movimiento = _context.MovimientosStock
                .Include(m => m.Producto)
                .Include(m => m.Usuario)
                .FirstOrDefault(m => m.IdMovimiento == id);
            if (movimiento == null) return NotFound();

            return View(movimiento);
        }

        [HttpPost, ActionName("Delete")]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Empleado") && !User.IsInRole("Jefe"))
            {
                TempData["ErrorMovimientos"] = "No tienes permisos para eliminar movimientos.";
                return RedirectToAction("Index");
            }

            var movimiento = await _context.MovimientosStock.FindAsync(id);
            if (movimiento == null) return NotFound();

            var producto = await _context.Productos.FindAsync(movimiento.IdProducto);
            if (producto == null) return NotFound();

            
            if (movimiento.TipoMovimiento == "Entrada")
                producto.StockActual -= movimiento.Cantidad;
            else if (movimiento.TipoMovimiento == "Salida")
                producto.StockActual += movimiento.Cantidad;

            _context.MovimientosStock.Remove(movimiento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
