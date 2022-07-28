using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.AuditLog.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string Access = "AuditLog:access";
                public const string Create = "AuditLog:create";
                public const string Read = "AuditLog:read";
                public const string Update = "AuditLog:update";
                public const string Delete = "AuditLog:delete";

                public static string[] AllPermissions { get; } =
                {
                    Access,
                    Create,
                    Read,
                    Update,
                    Delete,
                };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static SettingDescriptor AuditLogTrackingEvents { get; } = new SettingDescriptor
                {
                    Name = "AuditLog.AuditLogTrackingEvents",
                    GroupName = "AuditLog|Advanced",
                    ValueType = SettingValueType.LongText,
                    RestartRequired = true,
                    IsHidden = true,
                };

                public static IEnumerable<SettingDescriptor> AllGeneralSettings
                {
                    get
                    {
                        yield return AuditLogTrackingEvents;
                    }
                }
            }

            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    return General.AllGeneralSettings;
                }
            }
        }
    }
}
