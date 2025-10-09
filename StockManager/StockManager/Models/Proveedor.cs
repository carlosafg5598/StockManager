using System.ComponentModel.DataAnnotations;

namespace StockManager.Models
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreProveedor { get; set; }

        [MaxLength(50)]
        public string? TelefonoProveedor { get; set; }

        [MaxLength(100)]
        public string? EmailProveedor { get; set; }

        [MaxLength(255)]
        public string? DireccionProveedor { get; set; }

        //Relación con Productos
        public List<Producto>? Productos { get; set; }
    }
}
