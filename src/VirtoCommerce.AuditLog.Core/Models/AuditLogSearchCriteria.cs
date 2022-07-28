using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.AuditLog.Core.Models
{
    public class AuditLogSearchCriteria : SearchCriteriaBase
    {
        public IList<EntryState> OperationTypes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IncludeFields { get; set; }
    }
}
