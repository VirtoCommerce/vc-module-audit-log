using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.AuditLog.Core.Models
{
    public class AuditLogRecordField : Entity
    {
        public string Name { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
