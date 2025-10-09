using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManager.Models
{
    public class Producto
    {
        [Key] 
        public int IdProducto { get; set; }


        [Required]
        [MaxLength(100)]
        public string NombreProducto {  get; set; }
        
        
        [MaxLength(250)]
        public string? DescripcionProducto {  get; set; }
        
        
        [Column(TypeName ="decimal(18,2)")]
        public decimal PrecioProducto {  get; set; }
        
        
        public int idProovedor {  get; set; }
        
        
        public int StockActual {  get; set; }
        
        
        public bool Activo {  get; set; }


        //Relación con Proveedor
        public Proveedor? Proveedor { get; set; }

        //Relación con Movimientos
        public List<MovimientoStock>? Movimientos { get; set; }
    }
}
