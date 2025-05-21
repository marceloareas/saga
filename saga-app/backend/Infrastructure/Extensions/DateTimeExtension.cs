using System.Globalization;
using System.Text.RegularExpressions;

namespace saga.Infrastructure.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime? Parse(this string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            string pattern = @"^\d{2}/\d{2}/\d{4}$";
            string format = "dd/MM/yyyy";

            bool isValidFormat = Regex.IsMatch(dateString, pattern);
            DateTime date;

            if (isValidFormat && DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date.ToUniversalTime();
            }

            if (DateTime.TryParse(dateString, out date))
            {
                return date.ToUniversalTime();
            }

            return null;
        }
    }
}
