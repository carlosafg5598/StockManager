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
        [Display(Name = "Nombre del producto")]
        public string? NombreProducto {  get; set; }
        
        
        [MaxLength(250)]
        [Display(Name = "Decripcion del producto")]
        public string? DescripcionProducto {  get; set; }
        
        
        [Column(TypeName ="decimal(18,2)")]
        [Display(Name = "Precio del producto")]
        public decimal PrecioProducto {  get; set; }

        [Display(Name = "Proveedor")]
        public int? IdProveedor {  get; set; }
        
        
        public int StockActual {  get; set; }
        
        
        public bool Activo {  get; set; }


        //Relación con Proveedor
        [ForeignKey("IdProveedor")]
        public Proveedor? Proveedor { get; set; }

        //Relación con Movimientos
        public List<MovimientoStock>? Movimientos { get; set; }
    }
}
