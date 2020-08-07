using Microsoft.Extensions.Configuration;
using System;

namespace Shares.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static int GetInt(this IConfiguration configuration, string key)
        {
            if (!int.TryParse(configuration[key], out var value))
            {
                throw new InvalidOperationException($"Configuration value '{key}' is not an unsigned integer.");
            }

            return value;
        }

        public static int? GetNullInt(this IConfiguration configuration, string key)
        {
            if (!int.TryParse(configuration[key], out var value))
            {
                return null;
            }

            return value;
        }

        public static bool GetBool(this IConfiguration configuration, string key, bool defaultValue = false)
        {
            if (!bool.TryParse(configuration[key], out var value))
            {
                return defaultValue;
            }

            return value;
        }
    }
}
