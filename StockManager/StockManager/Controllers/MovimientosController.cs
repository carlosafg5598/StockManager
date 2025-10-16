




using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Models;
using StockManager.Models.Data;
using System.Linq;

namespace StockManager.Controllers
{
    [Authorize(Roles = "Administrador,Empleado,Jefe")]
    public class MovimientosController : Controller
    {
        private readonly AppDbContext _context;

        public MovimientosController(AppDbContext context)
        {
            _context = context;
        }

        // LISTADO DE MOVIMIENTOS
        public IActionResult Index()
        {
            var movimientos = _context.MovimientosStock
                                      .Include(m => m.Producto) // carga el producto relacionado
                                      .OrderByDescending(m => m.FechaMovimiento)
                                      .ToList();
            return View(movimientos);
        }

        // CREAR MOVIMIENTO - Formulario
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Empleado")]
        public IActionResult Create(MovimientoStock movimiento)
        {
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







        [HttpPost]
        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> Edit(MovimientoStock movimiento)
        {
            if (!ModelState.IsValid) return View(movimiento);

            _context.MovimientosStock.Update(movimiento);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> Delete(int id)
        {
            var movimiento = await _context.MovimientosStock.FindAsync(id);
            if (movimiento == null) return NotFound();

            _context.MovimientosStock.Remove(movimiento);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}

