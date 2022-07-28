using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.AuditLog.Core.Models;

namespace VirtoCommerce.AuditLog.Core.Services
{
    public interface IAuditLogService
    {
        Task<AuditLogRecord> GetAuditLogRecordById(string id);
        Task SaveChanges(IList<AuditLogRecord> records);
        Task<AuditLogSearchResult> Search(AuditLogSearchCriteria criteria);
    }
}
