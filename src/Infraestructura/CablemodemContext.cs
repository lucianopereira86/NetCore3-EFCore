using Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura
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

            modelBuider.Entity<Cablemodem>().ToTable("docsis_update");
            modelBuider.Entity<Cablemodem>().Property(c => c.MacAddress).HasColumnName("modem_macaddr");
            modelBuider.Entity<Cablemodem>().Property(c => c.Ip).HasColumnName("ipaddr");
            modelBuider.Entity<Cablemodem>().Property(c => c.Modelo).HasColumnName("vsi_model");
            modelBuider.Entity<Cablemodem>().Property(c => c.Fabricante).HasColumnName("vsi_vendor");
            modelBuider.Entity<Cablemodem>().Property(c => c.VersionSoftware).HasColumnName("vsi_swver");

            base.OnModelCreating(modelBuider);
        }
    }
}
