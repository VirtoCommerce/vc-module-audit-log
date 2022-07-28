using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.AuditLog.Data.Models;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.AuditLog.Data.Repositories
{
    public class AuditLogRepository : DbContextRepositoryBase<AuditLogDbContext>, IAuditLogRepository
    {
        public AuditLogRepository(AuditLogDbContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<AuditLogRecordEntity> AuditLogRecords => DbContext.Set<AuditLogRecordEntity>();

        public IQueryable<AuditLogRecordFieldEntity> AuditLogRecordFields => DbContext.Set<AuditLogRecordFieldEntity>();

        public async Task<AuditLogRecordEntity> GetAuditLogRecordById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            var result = await AuditLogRecords.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (result != null)
            {
                await AuditLogRecordFields.Where(x => x.AuditLogRecordId == id).LoadAsync();
            }

            return result;
        }
    }
}
