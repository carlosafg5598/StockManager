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
            var jefeEmail = "jefe@mail.com";
            if (await userManager.FindByEmailAsync(jefeEmail) == null)
            {
                var jefe = new ApplicationUser { UserName = jefeEmail, Email = jefeEmail, NombreCompleto = "Jefe General" };
                await userManager.CreateAsync(jefe, "1234");
                await userManager.AddToRoleAsync(jefe, "Jefe");
            }

            //Administrador
            var adminEmail = "admin@mail.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, NombreCompleto = "Administrador Almacén" };
                await userManager.CreateAsync(admin, "1234");
                await userManager.AddToRoleAsync(admin, "Administrador");
            }

            //Empleado
            var empleadoEmail = "empleado@mail.com";
            if (await userManager.FindByEmailAsync(empleadoEmail) == null)
            {
                var empleado = new ApplicationUser { UserName = empleadoEmail, Email = empleadoEmail, NombreCompleto = "Empleado de Planta" };
                await userManager.CreateAsync(empleado, "1234");
                await userManager.AddToRoleAsync(empleado, "Empleado");
            }
        }
    }
}
