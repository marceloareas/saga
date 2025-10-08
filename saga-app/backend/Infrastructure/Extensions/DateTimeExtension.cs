using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace saga.Infrastructure.Extensions
{
    public static class DateTimeExtension
    {
        // Converts any parsed date to UTC without throwing on Unspecified kinds.
        private static DateTime ToUtc(DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Utc) return dt;
            if (dt.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(dt, DateTimeKind.Local).ToUniversalTime();
            return dt.ToUniversalTime();
        }

        /// <summary>
        /// Tries to parse common date formats (pt-BR and ISO) and returns UTC. Null if invalid.
        /// </summary>
        public static DateTime? Parse(this string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            // Fast-path for dd/MM/yyyy
            const string brPattern = @"^\d{2}/\d{2}/\d{4}$";
            const string brFormat  = "dd/MM/yyyy";

            if (Regex.IsMatch(dateString, brPattern) &&
                DateTime.TryParseExact(dateString, brFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var brDate))
            {
                return ToUtc(brDate);
            }

            // Try a set of common exact formats first
            var formats = new[]
            {
                "dd/MM/yyyy", "dd-MM-yyyy",
                "dd/MM/yy",   "dd-MM-yy",
                "yyyy-MM-dd", "yyyy/MM/dd",
                "yyyy-MM-ddTHH:mm:ss.FFFFFFFK", // ISO 8601 variants
                "yyyy-MM-ddTHH:mm:ssK",
                "MM/dd/yyyy"
            };

            if (DateTime.TryParseExact(dateString, formats,
                    CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var exact))
            {
                return ToUtc(exact);
            }

            // Fallback to culture-based parsing (pt-BR then invariant)
            if (DateTime.TryParse(dateString, CultureInfo.GetCultureInfo("pt-BR"),
                    DateTimeStyles.AllowWhiteSpaces, out var br))
            {
                return ToUtc(br);
            }

            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture,
                    DateTimeStyles.AllowWhiteSpaces, out var any))
            {
                return ToUtc(any);
            }

            return null;
        }
    }
}
