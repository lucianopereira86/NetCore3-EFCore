using Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Impl
{
    public class CablemodemContext : DbContext
    {
        public DbSet<Usuario> User { get; set; }
        public CablemodemContext(DbContextOptions<CablemodemContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuider)
        {
            modelBuider.Entity<Usuario>()
                .ToTable("user")
                .HasKey(k => k.Id);

            base.OnModelCreating(modelBuider);
        }
    }
}
