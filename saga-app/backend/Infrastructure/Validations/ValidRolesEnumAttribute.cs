using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using saga.Models.Enums;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// A custom validation attribute used to validate roles enum values.
    /// </summary>
    public class ValidRolesEnumAttribute : ValidationAttribute
    {
        private readonly IEnumerable<RolesEnum> _acceptedRoles;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidRolesEnumAttribute"/> class.
        /// </summary>
        /// <param name="acceptedRoles">The accepted roles enum values.</param>
        public ValidRolesEnumAttribute(params RolesEnum[] acceptedRoles)
        {
            _acceptedRoles = acceptedRoles;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidRolesEnumAttribute"/> class.
        /// </summary>
        /// <param name="acceptedRole">The accepted role enum value.</param>
        public ValidRolesEnumAttribute(RolesEnum acceptedRole)
        {
            _acceptedRoles = new List<RolesEnum> { acceptedRole };
        }

        /// <summary>
        /// Determines whether the specified value is a valid roles enum value.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the value is valid or not.</returns>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || !Enum.IsDefined(typeof(RolesEnum), value))
            {
                return new ValidationResult($"The {validationContext.DisplayName} field is not a valid {typeof(RolesEnum).Name} value.");
            }

            if (Enum.TryParse(value.ToString(), out RolesEnum enumValue) && !_acceptedRoles.Contains(enumValue))
            {
                return new ValidationResult($"The {validationContext.DisplayName} does not accept role {value}.");
            }

            return ValidationResult.Success;
        }
    }
}
