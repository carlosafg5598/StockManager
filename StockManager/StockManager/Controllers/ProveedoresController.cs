using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Models.Data; 
using StockManager.Models;     
using Microsoft.AspNetCore.Authorization;

namespace StockManager.Controllers
{
    [Authorize(Roles = "Administrador,Jefe")] 
    public class ProveedoresController : Controller
    {
        private readonly AppDbContext _context;

        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var proveedores = _context.Proveedores
                .Include(p => p.Productos) 
                .ToList();

            return View(proveedores);
        }

        [Authorize(Roles = "Administrador,Jefe")]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Jefe")]
        public IActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Proveedores.Add(proveedor);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            
            return View(proveedor);
        }

        
        [Authorize(Roles = "Administrador,Jefe")]
        public IActionResult Edit(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Jefe")]
        public IActionResult Edit(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Proveedores.Update(proveedor);
                _context.SaveChanges();
                
                return RedirectToAction(nameof(Index));
            }

            
            return View(proveedor);
        }

        
        [Authorize(Roles = "Administrador,Jefe")]
        public IActionResult Delete(int id)
        {
            var proveedor = _context.Proveedores
                .Include(p => p.Productos)
                .FirstOrDefault(p => p.IdProveedor == id);

            if (proveedor == null) return NotFound();

            return View(proveedor);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Jefe")]
        public IActionResult DeleteConfirmed(int id)
        {
            var proveedor = _context.Proveedores
                .Include(p => p.Productos)
                .FirstOrDefault(p => p.IdProveedor == id);

            if (proveedor == null) return NotFound();

            if (proveedor.Productos != null && proveedor.Productos.Any())
            {
                ModelState.AddModelError("", "No puedes eliminar este proveedor porque tiene productos asociados.");
                return View("Delete", proveedor);
            }

            _context.Proveedores.Remove(proveedor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}

