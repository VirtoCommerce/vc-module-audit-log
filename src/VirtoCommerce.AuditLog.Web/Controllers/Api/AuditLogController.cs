using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.AuditLog.Core;

namespace VirtoCommerce.AuditLog.Web.Controllers.Api
{
    [Route("api/audit-log")]
    public class AuditLogController : Controller
    {
        // GET: api/audit-log
        /// <summary>
        /// Get message
        /// </summary>
        /// <remarks>Return "Hello world!" message</remarks>
        [HttpGet]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public ActionResult<string> Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
