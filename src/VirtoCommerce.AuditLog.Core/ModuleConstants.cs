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
                public static SettingDescriptor AuditLogEnabled { get; } = new SettingDescriptor
                {
                    Name = "AuditLog.AuditLogEnabled",
                    GroupName = "AuditLog|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false,
                };

                public static SettingDescriptor AuditLogPassword { get; } = new SettingDescriptor
                {
                    Name = "AuditLog.AuditLogPassword",
                    GroupName = "AuditLog|Advanced",
                    ValueType = SettingValueType.SecureString,
                    DefaultValue = "qwerty",
                };

                public static IEnumerable<SettingDescriptor> AllGeneralSettings
                {
                    get
                    {
                        yield return AuditLogEnabled;
                        yield return AuditLogPassword;
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
