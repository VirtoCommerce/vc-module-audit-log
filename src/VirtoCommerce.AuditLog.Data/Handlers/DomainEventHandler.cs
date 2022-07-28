using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.AuditLog.Core.Extensions;
using VirtoCommerce.AuditLog.Core.Handlers;
using VirtoCommerce.AuditLog.Core.Models;
using VirtoCommerce.AuditLog.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.AuditLog.Data.Handlers
{
    public class DomainEventHandler : IDomainEventHandler
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IAuditLogService _auditLogService;
        private readonly string[] _untrackedPropertyNames = { "Id", "ModifiedDate", "ModifiedBy", "CreatedDate", "CreatedBy" };

        public DomainEventHandler(ISettingsManager settingsManager, IAuditLogService auditLogService)
        {
            _settingsManager = settingsManager;
            _auditLogService = auditLogService;
        }

        public virtual async Task Handle(DomainEvent domainEvent)
        {
            var eventNames = await _settingsManager.GetTrackingEventNames();

            if (eventNames.Contains(domainEvent.GetType().FullName))
            {
                await HandleTrackingEvent(domainEvent);
            }
        }


        protected virtual async Task HandleTrackingEvent(DomainEvent domainEvent)
        {
            var records = GetPropertyValue<IEnumerable<GenericChangedEntry<IEntity>>>(domainEvent, nameof(GenericChangedEntryEvent<IEntity>.ChangedEntries))
                ?.Where(x => x != null)
                .Select(GetAuditLogRecord)
                .ToArray() ?? Array.Empty<AuditLogRecord>();

            await _auditLogService.SaveChanges(records);
        }


        protected virtual AuditLogRecord GetAuditLogRecord<TModel>(GenericChangedEntry<TModel> changedEntry)
            where TModel : IEntity
        {
            var entryState = GetPropertyValue<EntryState>(changedEntry, nameof(GenericChangedEntry<TModel>.EntryState));
            var oldEntry = GetPropertyValue<TModel>(changedEntry, nameof(GenericChangedEntry<TModel>.OldEntry));
            var newEntry = GetPropertyValue<TModel>(changedEntry, nameof(GenericChangedEntry<TModel>.NewEntry));

            var record = AbstractTypeFactory<AuditLogRecord>.TryCreateInstance();
            record.OperationType = entryState;
            record.ObjectId = oldEntry?.Id ?? newEntry?.Id;
            record.ObjectType = oldEntry.GetType().Name;

            if (entryState == EntryState.Modified || entryState == EntryState.Deleted)
            {
                if (oldEntry != null)
                {
                    record.OldObject = JsonConvert.SerializeObject(oldEntry);
                }

                if (entryState == EntryState.Modified)
                {
                    record.Fields = GetChangedFields(oldEntry, newEntry);
                    record.Details = GetAuditLogRecordDetails(record.Fields);
                }
                else
                {
                    record.Details = $"The {record.ObjectType} with id - {record.ObjectId} was deleted.";
                }
            }
            else if (entryState == EntryState.Added)
            {
                record.Details = $"The {record.ObjectType} with id - {record.ObjectId} was created.";
            }

            return record;
        }

        protected virtual T GetPropertyValue<T>(object entity, string propertyName)
        {
            T result = default;
            var property = entity.GetType().GetProperties().FirstOrDefault(x => x.Name == propertyName);
            var value = property.GetValue(entity);

            if (value != null)
            {
                result = (T)value;
            }

            return result;
        }

        protected virtual IList<AuditLogRecordField> GetChangedFields<T>(T oldEntry, T newEntry)
        {
            var result = new List<AuditLogRecordField>();

            var properties = oldEntry
                .GetType()
                .GetProperties()
                .Where(x => !_untrackedPropertyNames.Contains(x.PropertyType.Name) &&
                            (x.PropertyType != typeof(string) || !x.PropertyType.IsAssignableTo(typeof(IEnumerable))));

            foreach (var property in properties)
            {
                var oldValue = property.GetValue(oldEntry);
                var newValue = property.GetValue(newEntry);

                if ((oldValue == null && newValue != null) || (oldValue != null && !oldValue.Equals(newValue)))
                {
                    var field = new AuditLogRecordField
                    {
                        Name = property.Name,
                        OldValue = oldValue?.ToString(),
                        NewValue = newValue?.ToString(),
                    };

                    result.Add(field);
                }
            }

            return result;
        }

        protected virtual string GetAuditLogRecordDetails(IList<AuditLogRecordField> fields)
        {
            var builder = new StringBuilder();

            foreach (var field in fields)
            {
                builder.AppendLine($"Property '{field.Name}' value was changed from '{field.OldValue}' to '{field.NewValue}'.");
            }

            return builder.ToString();
        }
    }
}
