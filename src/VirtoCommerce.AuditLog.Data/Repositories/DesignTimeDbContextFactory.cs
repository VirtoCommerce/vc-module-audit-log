using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.AuditLog.Data.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuditLogDbContext>
    {
        public AuditLogDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AuditLogDbContext>();

            builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new AuditLogDbContext(builder.Options);
        }
    }
}
