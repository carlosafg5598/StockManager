using Microsoft.AspNetCore.Identity;

namespace StockManager.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string NombreCompleto {  get; set; }=string.Empty;
    }
}
