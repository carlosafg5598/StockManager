using Microsoft.AspNetCore.Identity;

namespace StockManager.Models.Data
{
    public class SeedIdentityData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //Creamos los roles
            string[] roleNames = { "Jefe", "Administrador", "Empleado" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Creamos un usuario por rol para pruebas

            //Jefe
            var jefeEmail = "curro@mail.com";
            if (await userManager.FindByEmailAsync(jefeEmail) == null)
            {
                var jefe = new ApplicationUser { UserName = jefeEmail, Email = jefeEmail, NombreCompleto = "Curro" };
                await userManager.CreateAsync(jefe, "1234");
                await userManager.AddToRoleAsync(jefe, "Jefe");
            }

            
        }
    }
}
