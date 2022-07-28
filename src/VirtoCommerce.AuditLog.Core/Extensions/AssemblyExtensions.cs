using System;
using System.Reflection;

namespace VirtoCommerce.AuditLog.Core.Extensions
{
    public static class AssemblyExtensions
    {
        public static Type[] GetTypesSafe(this Assembly assembly)
        {
            var result = Array.Empty<Type>();

            try
            {
                result = assembly.GetTypes();
            }
            catch (Exception ex) when (ex is ReflectionTypeLoadException || ex is TypeLoadException)
            {
                // No need to trow as we could have exceptions when loading types
            }

            return result;
        }
    }
}
