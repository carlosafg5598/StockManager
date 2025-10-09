namespace StockManager.Models.Data
{
    public class Seed
    {
        public static void Initialize(AppDbContext context)
        {
            // Asegúrate de que la base de datos esté creada
            context.Database.EnsureCreated();

            // ----- USUARIOS -----
            if (!context.Usuarios.Any())
            {
                var usuarios = new Usuario[]
                {
                new Usuario {
                    NombreUsuario = "Juan Perez",
                    EmailUsuario = "juanperez@mail.com",
                    PasswordHashUsuario = "1234hashed", // Para pruebas
                    RolUsuario = "Administrador"
                },
                new Usuario {
                    NombreUsuario = "Ana Gomez",
                    EmailUsuario = "anagomez@mail.com",
                    PasswordHashUsuario = "5678hashed",
                    RolUsuario = "Empleado"
                }
                };

                context.Usuarios.AddRange(usuarios);
                context.SaveChanges();
            }

            // ----- PRODUCTOS -----
            if (!context.Productos.Any())
            {
                var productos = new Producto[]
                {
                new Producto {
                    NombreProducto = "Mouse Gamer",
                    DescripcionProducto = "Mouse con iluminación RGB",
                    PrecioProducto = 25.50M,
                    idProovedor = 1,
                    StockActual = 10,
                    Activo = true
                },
                new Producto {
                    NombreProducto = "Teclado Mecánico",
                    DescripcionProducto = "Teclado mecánico azul",
                    PrecioProducto = 45.00M,
                    idProovedor = 1,
                    StockActual = 5,
                    Activo = true
                }
                };

                context.Productos.AddRange(productos);
                context.SaveChanges();
            }
        }
    }
}
