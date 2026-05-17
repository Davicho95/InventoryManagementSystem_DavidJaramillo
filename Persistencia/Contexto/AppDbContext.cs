using Aplicacion.Interfaces;
using Dominio.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia.Contexto
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly ICurrentUserService? _currentUserService;
        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService? currentUserService = null) : base(options) { _currentUserService = currentUserService; }

        #region Tablas
        public DbSet<Product> Products => Set<Product>();
        public DbSet<StockMovement> StockMovements => Set<StockMovement>();
        #endregion

        // aquí se llena CreatedBy y UpdatedBy
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var usuarioActual = _currentUserService?.Email ?? "system";

            foreach (var entry in ChangeTracker.Entries<Product>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = usuarioActual;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = usuarioActual;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

            builder.Entity<IdentityUserRole<string>>()
                   .HasKey(r => new { r.UserId, r.RoleId });

            builder.Entity<IdentityUserToken<string>>()
                   .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            builder.Entity<IdentityUserPasskey<string>>()
                   .HasKey(p => new { p.UserId, p.CredentialId });

            builder.Entity<IdentityUserPasskey<string>>(e =>
            {
                e.Ignore(p => p.Data);
            });

            builder.Ignore<IdentityPasskeyData>();

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
