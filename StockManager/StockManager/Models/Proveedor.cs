using System.ComponentModel.DataAnnotations;

namespace StockManager.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(50)]
        public string? Telefono { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(255)]
        public string? Direccion { get; set; }

        // 🔗 Relación con Productos
        public ICollection<Producto>? Productos { get; set; }
    }
}
