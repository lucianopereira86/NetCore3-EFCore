using Microsoft.EntityFrameworkCore;
using NetCore3WebAPI.Infra.Models;

namespace NetCore3WebAPI.Infra.Repository
{
    public class DBContext: DbContext
    {
        public DbSet<User> User { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuider)
        {
            modelBuider.Entity<User>(e =>
            {
                e
                .ToTable("user")
                .HasKey(k => k.id);

                e
                .Property(p => p.id)
                .ValueGeneratedOnAdd();
            });
            base.OnModelCreating(modelBuider);
        }
    }
}
