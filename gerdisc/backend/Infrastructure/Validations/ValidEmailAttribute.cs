using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// A custom validation attribute used to validate email addresses.
    /// </summary>
    public class ValidEmailAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value is a valid email address.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><c>true</c> if the value is a valid email address; otherwise, <c>false</c>.</returns>
        public override bool IsValid(object? value)
        {
            var email = value as string;
            if (email == null) return false;

            // Your custom validation logic goes here
            // For example, you can use a regular expression to validate the email format
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
    }
}
