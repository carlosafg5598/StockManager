using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Models.Data;
using System;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;


namespace StockManager.Controllers
{
    [Authorize(Roles = "Administrador,Jefe")]
    public class InformesController : Controller
    {
        private readonly AppDbContext _context;

        public InformesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string tipoMovimiento, DateTime? desde, DateTime? hasta)
        {
            
            var totalProductos = _context.Productos.Count();
            var totalProveedores = _context.Proveedores.Count();
            var totalMovimientos = _context.MovimientosStock.Count();
            var productosBajoStock = _context.Productos
                .Include(p => p.Proveedor)
                .Where(p => p.StockActual < 5)
                .ToList();

            
            var movimientos = _context.MovimientosStock
                .Include(m => m.Producto)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tipoMovimiento))
                movimientos = movimientos.Where(m => m.TipoMovimiento == tipoMovimiento);

            if (desde.HasValue)
                movimientos = movimientos.Where(m => m.FechaMovimiento >= desde.Value);

            if (hasta.HasValue)
                movimientos = movimientos.Where(m => m.FechaMovimiento <= hasta.Value);

            movimientos = movimientos.OrderByDescending(m => m.FechaMovimiento);

            ViewBag.TotalProductos = totalProductos;
            ViewBag.TotalProveedores = totalProveedores;
            ViewBag.TotalMovimientos = totalMovimientos;
            ViewBag.ProductosBajoStock = productosBajoStock;
            ViewBag.Movimientos = movimientos.ToList();

            return View();
        }


        public IActionResult ExportarPDF(string tipoMovimiento, DateTime? desde, DateTime? hasta)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var movimientos = _context.MovimientosStock
                .Include(m => m.Producto)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tipoMovimiento))
                movimientos = movimientos.Where(m => m.TipoMovimiento == tipoMovimiento);

            if (desde.HasValue)
                movimientos = movimientos.Where(m => m.FechaMovimiento >= desde.Value);

            if (hasta.HasValue)
                movimientos = movimientos.Where(m => m.FechaMovimiento <= hasta.Value);

            var lista = movimientos.OrderByDescending(m => m.FechaMovimiento).ToList();

            
            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    
                    page.Header()
                        .Text("Informe de Movimientos - StockManager")
                        .SemiBold().FontSize(18).AlignCenter();

                    
                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        col.Item().Text(text =>
                        {
                            text.Span("Fecha de generación: ").SemiBold();
                            text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        });

                        if (!string.IsNullOrEmpty(tipoMovimiento))
                        {
                            col.Item().Text(text =>
                            {
                                text.Span("Tipo de movimiento: ").SemiBold();
                                text.Span(tipoMovimiento);
                            });
                        }

                        if (desde.HasValue || hasta.HasValue)
                        {
                            col.Item().Text(text =>
                            {
                                text.Span("Rango de fechas: ").SemiBold();
                                text.Span($"{desde?.ToString("dd/MM/yyyy") ?? "-"} a {hasta?.ToString("dd/MM/yyyy") ?? "-"}");
                            });
                        }

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        
                        col.Item().Table(table =>
                        {
                            
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(80);   
                                columns.RelativeColumn(2);    
                                columns.ConstantColumn(80);   
                                columns.ConstantColumn(60);   
                            });

                            
                            table.Header(header =>
                            {
                                header.Cell().Text("Fecha").SemiBold();
                                header.Cell().Text("Producto").SemiBold();
                                header.Cell().Text("Tipo").SemiBold();
                                header.Cell().Text("Cantidad").SemiBold();
                            });

                            
                            foreach (var m in lista)
                            {
                                table.Cell().Text(m.FechaMovimiento.ToString("dd/MM/yyyy"));
                                table.Cell().Text(m.Producto?.NombreProducto ?? "N/A");
                                table.Cell().Text(m.TipoMovimiento);
                                table.Cell().Text(m.Cantidad.ToString());
                            }
                        });

                        if (!lista.Any())
                        {
                            col.Item().PaddingTop(20).Text("No hay movimientos que coincidan con los filtros.").Italic().FontColor(Colors.Grey.Medium);
                        }
                    });

                    
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Página ").FontSize(10);
                            x.CurrentPageNumber().FontSize(10);
                            x.Span(" de ").FontSize(10);
                            x.TotalPages().FontSize(10);
                        });
                });
            });

            
            var pdfBytes = doc.GeneratePdf();

            
            return File(pdfBytes, "application/pdf", "InformeMovimientos.pdf");
        }

    }
}
