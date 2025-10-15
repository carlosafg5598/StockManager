//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Identity.Client;
//using StockManager.Models;
//using StockManager.Models.Data;

//namespace StockManager.Controllers
//{
//    public class MovimientosController : Controller
//    {

//        private readonly AppDbContext _context;

//        public MovimientosController(AppDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index()
//        {
//            var movimientos = _context.MovimientosStock.Include(m => m.Producto).OrderByDescending(m => m.FechaMovimiento).ToList();
//            return View(movimientos);
//        }

//        public IActionResult Create()
//        {
//            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
//            return View();
//        }

//        [HttpPost]
//        public IActionResult Create(MovimientoStock movimiento)
//        {
//            if (ModelState.IsValid)
//            {
//                movimiento.FechaMovimiento = DateTime.Now;
//                _context.MovimientosStock.Add(movimiento);

//                // Actualizar stock del producto
//                var producto = _context.Productos.Find(movimiento.IdProducto);
//                if (movimiento.TipoMovimiento == "Entrada")
//                    producto.StockActual += movimiento.Cantidad;
//                else if (movimiento.TipoMovimiento == "Salida")
//                    producto.StockActual -= movimiento.Cantidad;

//                _context.SaveChanges();
//                return RedirectToAction(nameof(Index));
//            }


//            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
//            return View(movimiento);
//        }

//        // OPCIONAL: Delete
//        public IActionResult Delete(int id)
//        {
//            var movimiento = _context.MovimientosStock
//                                     .Include(m => m.Producto)
//                                     .FirstOrDefault(m => m.IdMovimiento == id);
//            if (movimiento == null) return NotFound();
//            return View(movimiento);
//        }

//        [HttpPost, ActionName("Delete")]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            var movimiento = _context.MovimientosStock.Find(id);
//            _context.MovimientosStock.Remove(movimiento);
//            _context.SaveChanges();
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}




using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Models;
using StockManager.Models.Data;
using System.Linq;

namespace StockManager.Controllers
{
    [Authorize(Roles ="Administrador,Empleado,Jefe")]
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
        [Authorize(Roles ="Empleado")]
        public IActionResult Create()
        {
            ViewBag.Productos = _context.Productos.Where(p => p.Activo).ToList();
            return View();
        }

        // CREAR MOVIMIENTO - POST
        [HttpPost]
        [Authorize(Roles = "Empleado")]
        public IActionResult Create(MovimientoStock movimiento)
        {

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("ERROR: " + error.ErrorMessage);
            }

            if (ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var e in errores)
                    Console.WriteLine("Error: " + e.ErrorMessage);
                // Fecha actual
                movimiento.FechaMovimiento = DateTime.Now;

                // Agregar movimiento
                _context.MovimientosStock.Add(movimiento);

                // Actualizar stock
                var producto = _context.Productos.Find(movimiento.IdProducto);
                if (producto != null)
                {
                    if (movimiento.TipoMovimiento == "Entrada")
                        producto.StockActual += movimiento.Cantidad;
                    else if (movimiento.TipoMovimiento == "Salida")
                    {
                        if (movimiento.Cantidad > producto.StockActual)
                            movimiento.Cantidad = producto.StockActual; // no permitir stock negativo
                        producto.StockActual -= movimiento.Cantidad;
                    }
                }

                // Guardar cambios en la base de datos
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }


            // Si hay error, recargar lista de productos
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

        //// OPCIONAL: Delete
        //[Authorize(Roles = "Administrador,Jefe")]
        //public IActionResult Delete(int id)
        //{
        //    var movimiento = _context.MovimientosStock
        //                             .Include(m => m.Producto)
        //                             .FirstOrDefault(m => m.IdMovimiento == id);
        //    if (movimiento == null) return NotFound();
        //    return View(movimiento);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var movimiento = _context.MovimientosStock.Find(id);
        //    _context.MovimientosStock.Remove(movimiento);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}

