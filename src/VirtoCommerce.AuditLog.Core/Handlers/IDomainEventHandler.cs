using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.AuditLog.Core.Handlers
{
    public interface IDomainEventHandler
    {
        Task Handle(DomainEvent domainEvent);
    }
}
