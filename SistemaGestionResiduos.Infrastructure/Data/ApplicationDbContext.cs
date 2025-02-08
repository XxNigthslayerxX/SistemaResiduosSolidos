using Microsoft.EntityFrameworkCore;
using SistemaGestionResiduos.Domain.Entities;

namespace SistemaGestionResiduos.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Contenedor> Contenedores { get; set; }
        public DbSet<Recoleccion> Recolecciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ruta>()
                .HasMany(r => r.Contenedores)
                .WithOne(c => c.Ruta)
                .HasForeignKey(c => c.RutaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Contenedor>()
                .HasMany(c => c.Recolecciones)
                .WithOne(r => r.Contenedor)
                .HasForeignKey(r => r.ContenedorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
