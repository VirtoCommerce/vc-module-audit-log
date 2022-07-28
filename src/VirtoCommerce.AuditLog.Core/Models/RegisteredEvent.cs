using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.AuditLog.Core.Models
{
    public class RegisteredEvent : Entity
    {
        public Type EventType { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
