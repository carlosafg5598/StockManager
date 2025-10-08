using System.ComponentModel.DataAnnotations;

namespace StockManager.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(50)]
        public string Rol { get; set; }  // Ej: "Administrador", "Empleado"

        // 🔗 Relación con movimientos
        public ICollection<MovimientoStock>? Movimientos { get; set; }
    }
}
