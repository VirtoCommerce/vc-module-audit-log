using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.AuditLog.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.AuditLog.Data.Models
{
    public class AuditLogRecordFieldEntity : Entity
    {
        [StringLength(200)]
        public string Name { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string AuditLogRecordId { get; set; }
        public virtual AuditLogRecordEntity AuditLogRecord { get; set; }

        public virtual AuditLogRecordField ToModel(AuditLogRecordField field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            field.Id = Id;

            field.Name = Name;
            field.OldValue = OldValue;
            field.NewValue = NewValue;

            return field;
        }

        public virtual AuditLogRecordFieldEntity FromModel(AuditLogRecordField record, PrimaryKeyResolvingMap map)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            map.AddPair(record, this);

            Id = record.Id;

            Name = record.Name;
            OldValue = record.OldValue;
            NewValue = record.NewValue;

            return this;
        }
    }
}
