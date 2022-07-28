using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.AuditLog.Core.Extensions
{
    public static class SettingsManagerExtensions
    {
        public static async Task<IList<string>> GetTrackingEventNames(this ISettingsManager settingsManager)
        {
            var eventNamesString = await settingsManager.GetValueByDescriptorAsync<string>(ModuleConstants.Settings.General.AuditLogTrackingEvents);
            var eventNames = eventNamesString?.Split(',');

            return eventNames ?? Array.Empty<string>();
        }

        public static Task SetTrackingEventNames(this ISettingsManager settingsManager, IList<string> eventNames)
        {
            return settingsManager.SetValueAsync(ModuleConstants.Settings.General.AuditLogTrackingEvents.Name, string.Join(",", eventNames));
        }
    }
}
