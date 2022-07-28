using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.AuditLog.Core.Models
{
    public class AuditLogRecord : AuditableEntity
    {
        public EntryState OperationType { get; set; }
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public string Details { get; set; }
        public string OldObject { get; set; }
        public IList<AuditLogRecordField> Fields { get; set; }
    }
}
