using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace VirtoCommerce.AuditLog.Data.Repositories
{
    public class AuditLogDbContext : DbContextWithTriggers
    {
        public AuditLogDbContext(DbContextOptions<AuditLogDbContext> options)
            : base(options)
        {
        }

        protected AuditLogDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<AuditLogEntity>().ToTable("AuditLog").HasKey(x => x.Id);
            //modelBuilder.Entity<AuditLogEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
        }
    }
}
