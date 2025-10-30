using Microsoft.AspNetCore.Identity;

namespace StockManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;

        // Imagen de perfil (ruta al archivo)
        public string? FotoPerfil { get; set; }

        // Preferencias visuales
        public string? TemaColor { get; set; }
        public string? FuentePreferida { get; set; }
        public string? FondoPantalla { get; set; }
    }
}
