using Microsoft.AspNetCore.Identity;

namespace StockManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;

        
        public string? FotoPerfil { get; set; }

        public string? PhoneNumber { get; set; }


        public string? TemaColor { get; set; }
        public string? FuentePreferida { get; set; }
        public string? FondoPantalla { get; set; }
        public bool Activo { get; set; } = true;

    }
}
