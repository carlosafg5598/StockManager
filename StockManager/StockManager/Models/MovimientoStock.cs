using System.ComponentModel.DataAnnotations;

namespace StockManager.Models
{
    public class MovimientoStock
    {
        [Key]
        public int IdMovimiento { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(1)]
        public string TipoMovimiento { get; set; }  

        [Required]
        public int Cantidad { get; set; }

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        public int UsuarioId{ get; set; }

        //Relaciones
        public Producto Producto { get; set; }
        public Usuario Usuario { get; set; }
    }
}
