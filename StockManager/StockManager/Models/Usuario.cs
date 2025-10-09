using System.ComponentModel.DataAnnotations;

namespace StockManager.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string EmailUsuario { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHashUsuario { get; set; }

        [Required]
        [MaxLength(50)]
        public string RolUsuario { get; set; }  

        //Relación con movimientos
        public List<MovimientoStock>? Movimientos { get; set; }
    }
}
