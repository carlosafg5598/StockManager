using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManager.Models;
using StockManager.Models.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockManager.Controllers
{
    [Authorize(Roles = "Administrador,Jefe,Empleado")]
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Productos.ToListAsync());
        }

        
        public IActionResult Create()
        {
            if (User.IsInRole("Empleado"))
            {
                TempData["ErrorProductos"] = "No tienes permisos para crear productos.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Proveedores = new SelectList(
                _context.Proveedores.OrderBy(p => p.NombreProveedor).ToList(),
                "IdProveedor",
                "NombreProveedor"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            if (User.IsInRole("Empleado"))
            {
                TempData["ErrorProductos"] = "No tienes permisos para crear productos.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Proveedores = new SelectList(
                _context.Proveedores.OrderBy(p => p.NombreProveedor).ToList(),
                "IdProveedor",
                "NombreProveedor",
                producto.IdProveedor
            );

            return View(producto);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsInRole("Empleado"))
            {
                TempData["ErrorProductos"] = "No tienes permisos para editar productos.";
                return RedirectToAction(nameof(Index));
            }

            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            ViewBag.Proveedores = new SelectList(
                _context.Proveedores.OrderBy(p => p.NombreProveedor),
                "IdProveedor",
                "NombreProveedor",
                producto.IdProveedor
            );

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (User.IsInRole("Empleado"))
            {
                TempData["ErrorProductos"] = "No tienes permisos para editar productos.";
                return RedirectToAction(nameof(Index));
            }

            if (id != producto.IdProducto)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Productos.Any(e => e.IdProducto == producto.IdProducto))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole("Empleado"))
            {
                TempData["ErrorProductos"] = "No tienes permisos para eliminar productos.";
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
                return NotFound();

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.IdProducto == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole("Empleado"))
            {
                TempData["ErrorProductos"] = "No tienes permisos para eliminar productos.";
                return RedirectToAction(nameof(Index));
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
