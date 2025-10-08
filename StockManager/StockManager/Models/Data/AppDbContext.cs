using Microsoft.EntityFrameworkCore;

namespace StockManager.Models.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //TODO REVISAR CADA UNA DE LAS CLASES DE MODELS ANTES DE HACER LAS MIGRACIONES
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<MovimientoStock> MovimientosStock { get; set; }
    }
}
