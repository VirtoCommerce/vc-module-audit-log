using System.Collections.Generic;
using VirtoCommerce.AuditLog.Core.Models;

namespace VirtoCommerce.AuditLog.Core.Services
{
    public interface IGenericEventHandlerManager
    {
        IList<RegisteredEvent> GetAllEvents();
        void RegisterGenericHandler();
    }
}
