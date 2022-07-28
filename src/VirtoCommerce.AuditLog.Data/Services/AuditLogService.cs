using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.AuditLog.Core.Models;
using VirtoCommerce.AuditLog.Core.Services;
using VirtoCommerce.AuditLog.Data.Models;
using VirtoCommerce.AuditLog.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.AuditLog.Data.Services
{
    public class AuditLogService : IChangeLogService, IChangeLogSearchService, IAuditLogService
    {
        private readonly Func<IAuditLogRepository> _auditLogRepositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;

        public AuditLogService(Func<IAuditLogRepository> auditLogRepositoryFactory, IPlatformMemoryCache memoryCache)
        {
            _auditLogRepositoryFactory = auditLogRepositoryFactory;
            _memoryCache = memoryCache;
        }

        public virtual Task DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public virtual Task<OperationLog[]> GetByIdsAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public virtual Task SaveChangesAsync(params OperationLog[] operationLogs)
        {
            var records = operationLogs.Select(x => ConvertOperationLogToAuditLogRecord(x)).ToArray();
            return SaveChanges(records);
        }

        public virtual async Task SaveChanges(IList<AuditLogRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            var map = new PrimaryKeyResolvingMap();
            using var repository = _auditLogRepositoryFactory();

            foreach (var record in records)
            {
                var entity = AbstractTypeFactory<AuditLogRecordEntity>.TryCreateInstance().FromModel(record, map);
                repository.Add(entity);
            }

            await repository.UnitOfWork.CommitAsync();
        }

        public virtual async Task<ChangeLogSearchResult> SearchAsync(ChangeLogSearchCriteria criteria)
        {
            var result = AbstractTypeFactory<ChangeLogSearchResult>.TryCreateInstance();
            var auditLogSearchCriteria = ConvertSearchCriteria(criteria);

            var searchResult = await Search(auditLogSearchCriteria);
            result.Results = searchResult.Results.Select(r => ConvertAuditLogRecordToOperationLog(r)).ToList();
            result.TotalCount = searchResult.TotalCount;

            return result;
        }

        public virtual async Task<AuditLogSearchResult> Search(AuditLogSearchCriteria criteria)
        {
            var result = AbstractTypeFactory<AuditLogSearchResult>.TryCreateInstance();

            using var repository = _auditLogRepositoryFactory();
            repository.DisableChangesTracking();

            var query = BuildQuery(repository, criteria);
            var needExecuteCount = criteria.Take == 0;

            if (criteria.Take > 0)
            {
                var sortInfos = BuildSortExpression(criteria);

                if (criteria.Skip > 0 || result.TotalCount == criteria.Take)
                {
                    needExecuteCount = true;
                }

                var entities = await query
                    .OrderBySortInfos(sortInfos)
                    .ThenBy(x => x.Id)
                    .AsNoTracking()
                    .Skip(criteria.Skip)
                    .Take(criteria.Take)
                    .ToListAsync();

                result.Results = entities.Select(x => x.ToModel(AbstractTypeFactory<AuditLogRecord>.TryCreateInstance())).ToList();
                result.TotalCount = entities.Count;
            }

            if (needExecuteCount)
            {
                result.TotalCount = await query.CountAsync();
            }

            return result;
        }

        public virtual async Task<AuditLogRecord> GetAuditLogRecordById(string id)
        {
            using var repository = _auditLogRepositoryFactory();
            repository.DisableChangesTracking();

            var result = (await repository.GetAuditLogRecordById(id))?.ToModel(AbstractTypeFactory<AuditLogRecord>.TryCreateInstance());

            return result;
        }


        protected virtual IQueryable<AuditLogRecordEntity> BuildQuery(IAuditLogRepository repository, AuditLogSearchCriteria criteria)
        {
            var query = repository.AuditLogRecords
                .Where(x => (criteria.StartDate == null || x.ModifiedDate >= criteria.StartDate) &&
                            (criteria.EndDate == null || x.ModifiedDate <= criteria.EndDate));

            if (!criteria.OperationTypes.IsNullOrEmpty())
            {
                var operationTypes = criteria.OperationTypes.Select(x => x.ToString());
                query = query.Where(x => operationTypes.Contains(x.OperationType));
            }

            if (!criteria.ObjectIds.IsNullOrEmpty())
            {
                query = query.Where(x => criteria.ObjectIds.Contains(x.ObjectId));
            }

            if (!criteria.ObjectTypes.IsNullOrEmpty())
            {
                query = query.Where(x => criteria.ObjectTypes.Contains(x.ObjectType));
            }

            return query;
        }

        protected virtual IList<SortInfo> BuildSortExpression(AuditLogSearchCriteria criteria)
        {
            var sortInfos = criteria.SortInfos;

            if (sortInfos.IsNullOrEmpty())
            {
                sortInfos = new[]
                {
                    new SortInfo { SortColumn = nameof(OperationLog.ModifiedDate), SortDirection = SortDirection.Descending },
                };
            }

            return sortInfos;
        }

        protected virtual OperationLog ConvertAuditLogRecordToOperationLog(AuditLogRecord auditLogRecord)
        {
            var result = AbstractTypeFactory<OperationLog>.TryCreateInstance();

            result.Id = auditLogRecord.Id;

            result.CreatedDate = auditLogRecord.CreatedDate;
            result.ModifiedDate = auditLogRecord.ModifiedDate;
            result.CreatedBy = auditLogRecord.CreatedBy;
            result.ModifiedBy = auditLogRecord.ModifiedBy;

            result.OperationType = auditLogRecord.OperationType;
            result.ObjectId = auditLogRecord.ObjectId;
            result.ObjectType = auditLogRecord.ObjectType;
            result.Detail = auditLogRecord.Details;

            return result;
        }

        protected virtual AuditLogRecord ConvertOperationLogToAuditLogRecord(OperationLog operationLog)
        {
            var result = AbstractTypeFactory<AuditLogRecord>.TryCreateInstance();

            result.Id = operationLog.Id;

            result.CreatedDate = operationLog.CreatedDate;
            result.ModifiedDate = operationLog.ModifiedDate;
            result.CreatedBy = operationLog.CreatedBy;
            result.ModifiedBy = operationLog.ModifiedBy;

            result.OperationType = operationLog.OperationType;
            result.ObjectId = operationLog.ObjectId;
            result.ObjectType = operationLog.ObjectType;
            result.Details = operationLog.Detail;

            return result;
        }

        protected virtual AuditLogSearchCriteria ConvertSearchCriteria(ChangeLogSearchCriteria criteria)
        {
            var result = AbstractTypeFactory<AuditLogSearchCriteria>.TryCreateInstance();

            result.StartDate = criteria.StartDate;
            result.EndDate = criteria.EndDate;
            result.OperationTypes = criteria.OperationTypes;

            return result;
        }
    }
}
