using Microsoft.AspNetCore.Identity;

namespace StockManager.Models.Data
{
    public class SeedIdentityData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Administrador", "Encargado" };

            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@mail.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, NombreCompleto = "Administrador General" };
                await userManager.CreateAsync(admin, "1234");
                await userManager.AddToRoleAsync(admin, "Administrador");
            }
        }


    }
}
