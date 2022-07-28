using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.AuditLog.Data.Models;

namespace VirtoCommerce.AuditLog.Data.Repositories
{
    public class AuditLogDbContext : DbContextWithTriggers
    {
        private const int MaxLength = 128;

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

            modelBuilder.Entity<AuditLogRecordEntity>().ToTable("AuditLogRecord").HasKey(x => x.Id);
            modelBuilder.Entity<AuditLogRecordEntity>().Property(x => x.Id).HasMaxLength(MaxLength).ValueGeneratedOnAdd();

            modelBuilder.Entity<AuditLogRecordFieldEntity>().ToTable("AuditLogRecordField").HasKey(x => x.Id);
            modelBuilder.Entity<AuditLogRecordFieldEntity>().Property(x => x.Id).HasMaxLength(MaxLength).ValueGeneratedOnAdd();

            modelBuilder.Entity<AuditLogRecordFieldEntity>()
                .HasOne(x => x.AuditLogRecord)
                .WithMany(x => x.Fields)
                .HasForeignKey(x => x.AuditLogRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
