using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.AuditLog.Core;
using VirtoCommerce.AuditLog.Core.Extensions;
using VirtoCommerce.AuditLog.Core.Models;
using VirtoCommerce.AuditLog.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.AuditLog.Web.Controllers.Api
{
    [Route("api/audit-log")]
    public class AuditLogController : Controller
    {
        private readonly IGenericEventHandlerManager _genericEventHandlerManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IGenericEventHandlerManager genericEventHandlerManager,
            ISettingsManager settingsManager,
            IAuditLogService auditLogService)
        {
            _genericEventHandlerManager = genericEventHandlerManager;
            _settingsManager = settingsManager;
            _auditLogService = auditLogService;
        }

        [HttpGet("{id}")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<AuditLogRecord>> GetRecordById(string id)
        {
            var result = await _auditLogService.GetAuditLogRecordById(id);

            return Ok(result);
        }

        [HttpPost("")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<AuditLogRecord[]>> SearchRecords([FromBody] AuditLogSearchCriteria criteria)
        {
            var result = await _auditLogService.Search(criteria);

            return Ok(result);
        }

        [HttpGet("events")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<RegisteredEvent[]>> GetAvailableEvents()
        {
            var eventNames = await _settingsManager.GetTrackingEventNames();
            var events = _genericEventHandlerManager.GetAllEvents();
            events.Apply(x => x.IsActive = eventNames.Contains(x.Id));

            return Ok(events);
        }

        [HttpPost("events")]
        [Authorize(ModuleConstants.Security.Permissions.Update)]
        public async Task<ActionResult<RegisteredEvent[]>> UpdateEvents([FromBody] RegisteredEvent[] events)
        {
            await _settingsManager.SetTrackingEventNames(events.Select(x => x.Id).ToArray());

            return await GetAvailableEvents();
        }
    }
}
