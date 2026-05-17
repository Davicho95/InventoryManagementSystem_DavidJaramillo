using Dominio.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistencia.Contexto.Inicializacion
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                if (!context.Database.CanConnect())
                {
                    Console.WriteLine("La base de datos no existe, se procederá a crearla.");
                    context.Database.EnsureCreated();
                }
                // Aplicar migraciones pendientes (crea la BD si no existe)
                await context.Database.MigrateAsync();
                Console.WriteLine("Base de datos lista.");

                // Crear roles si no existen
                await SeedRolesAsync(roleManager);

                // Crear usuario admin si no existe
                await SeedAdminUserAsync(userManager);

                // Insertar productos de ejemplo si la tabla está vacía
                await SeedProductsAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
                throw;
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    Console.WriteLine($"Rol '{role}' creado.");
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager)
        {
            const string adminEmail = "admin@inventory.com";
            const string adminPassword = "Admin123!";

            // Si ya existe, no hace nada
            var existingUser = await userManager.FindByEmailAsync(adminEmail);
            if (existingUser != null) return;

            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"Usuario admin creado: {adminEmail}");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Console.WriteLine($"Error creando usuario admin: {errors}");
            }
        }

        private static async Task SeedProductsAsync(AppDbContext context)
        {
            if (context.Products.Any()) return; // si ya tiene datos se omite

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Laptop Dell XPS 15",
                    SKU = "DELL-XPS-15",
                    Category = "Electronics",
                    QuantityInStock = 25,
                    UnitPrice = 1499.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Wireless Mouse",
                    SKU = "MS-WIRELESS-001",
                    Category = "Accessories",
                    QuantityInStock = 5,
                    UnitPrice = 29.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "USB-C Hub",
                    SKU = "HUB-USBC-7IN1",
                    Category = "Accessories",
                    QuantityInStock = 50,
                    UnitPrice = 49.99m,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            Console.WriteLine("Productos de ejemplo insertados.");
        }
    }
}
