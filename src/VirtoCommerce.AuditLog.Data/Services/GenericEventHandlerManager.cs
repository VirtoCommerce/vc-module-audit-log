using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.AuditLog.Core.Extensions;
using VirtoCommerce.AuditLog.Core.Handlers;
using VirtoCommerce.AuditLog.Core.Models;
using VirtoCommerce.AuditLog.Core.Services;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.AuditLog.Data.Services
{
    public class GenericEventHandlerManager : IGenericEventHandlerManager
    {
        private readonly IHandlerRegistrar _eventHandlerRegistrar;
        private readonly IDomainEventHandler _domainEventHandler;

        private IList<RegisteredEvent> _allEvents;
        private readonly StringComparison _ignoreCase = StringComparison.OrdinalIgnoreCase;
        private readonly IList<string> _ignoreAssemblies = new[] { "Microsoft.", "System." };

        public GenericEventHandlerManager(IHandlerRegistrar eventHandlerRegistrar, IDomainEventHandler domainEventHandler)
        {
            _eventHandlerRegistrar = eventHandlerRegistrar;
            _domainEventHandler = domainEventHandler;
        }

        public virtual IList<RegisteredEvent> GetAllEvents()
        {
            if (_allEvents == null)
            {
                _allEvents = DiscoverAllDomainEvents();
            }

            return _allEvents;
        }

        public virtual void RegisterGenericHandler()
        {
            foreach (var @event in GetAllEvents())
            {
                RegisterHandler(@event.EventType, _eventHandlerRegistrar);
            }
        }

        protected virtual IList<RegisteredEvent> DiscoverAllDomainEvents()
        {
            var eventBaseType = typeof(DomainEvent);

            var result = AppDomain.CurrentDomain.GetAssemblies()
                // Maybe there is a way to find platform and modules related assemblies
                .Where(assembly => _ignoreAssemblies.Any(name => assembly.FullName.StartsWith(name, _ignoreCase)))
                .SelectMany(x => x.GetTypesSafe())
                .Where(x => !x.IsAbstract && !x.IsGenericTypeDefinition && x.IsSubclassOf(eventBaseType))
                .Select(x => new RegisteredEvent()
                {
                    Id = x.FullName,
                    EventType = x,
                    Name = x.Name,
                })
                .Distinct()
                .ToArray();

            return result;
        }

        protected virtual void RegisterHandler(Type eventType, IHandlerRegistrar registrar)
        {
            var registerExecutorMethod = registrar
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.Name == nameof(IHandlerRegistrar.RegisterHandler))
                .Where(x => x.IsGenericMethod)
                .Where(x => x.GetGenericArguments().Length == 1)
                .Single(x => x.GetParameters().Length == 1)
                .MakeGenericMethod(eventType);

            Func<DomainEvent, CancellationToken, Task> handler = (domainEvent, cancellationToken) =>
            {
                return _domainEventHandler.Handle(domainEvent);
            };

            registerExecutorMethod.Invoke(registrar, new object[] { handler });
        }
    }
}
