using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.AuditLog.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.AuditLog.Data.Models
{
    public class AuditLogRecordEntity : AuditableEntity
    {
        [Required]
        [StringLength(20)]
        public string OperationType { get; set; }

        [StringLength(50)]
        public string ObjectType { get; set; }

        [StringLength(200)]
        public string ObjectId { get; set; }

        public string Details { get; set; }

        public string OldObject { get; set; }

        public virtual ObservableCollection<AuditLogRecordFieldEntity> Fields { get; set; } = new NullCollection<AuditLogRecordFieldEntity>();

        public virtual AuditLogRecord ToModel(AuditLogRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            record.Id = Id;

            record.CreatedDate = CreatedDate;
            record.ModifiedDate = ModifiedDate;
            record.CreatedBy = CreatedBy;
            record.ModifiedBy = ModifiedBy;

            record.OperationType = EnumUtility.SafeParse(OperationType, EntryState.Unchanged);
            record.ObjectId = ObjectId;
            record.ObjectType = ObjectType;
            record.Details = Details;

            record.Fields = Fields.Select(x => x.ToModel(AbstractTypeFactory<AuditLogRecordField>.TryCreateInstance())).ToList();

            return record;
        }

        public virtual AuditLogRecordEntity FromModel(AuditLogRecord record, PrimaryKeyResolvingMap map)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            map.AddPair(record, this);

            Id = record.Id;
            CreatedDate = record.CreatedDate;
            ModifiedDate = record.ModifiedDate;
            CreatedBy = record.CreatedBy;
            ModifiedBy = record.ModifiedBy;

            OperationType = record.OperationType.ToString();
            ObjectId = record.ObjectId;
            ObjectType = record.ObjectType;
            Details = record.Details;
            OldObject = record.OldObject;

            if (record.Fields != null)
            {
                Fields = new ObservableCollection<AuditLogRecordFieldEntity>(record.Fields.Select(x =>
                    AbstractTypeFactory<AuditLogRecordFieldEntity>.TryCreateInstance().FromModel(x, map)));
            }

            return this;
        }
    }
}
