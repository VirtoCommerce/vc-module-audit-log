using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.AuditLog.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.AuditLog.Data.Repositories
{
    public interface IAuditLogRepository : IRepository
    {
        IQueryable<AuditLogRecordEntity> AuditLogRecords { get; }

        IQueryable<AuditLogRecordFieldEntity> AuditLogRecordFields { get; }

        Task<AuditLogRecordEntity> GetAuditLogRecordById(string id);
    }
}
