using System;
using System.ComponentModel.DataAnnotations;

namespace saga.Infrastructure.Validations
{
    public class ValidEnumAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public ValidEnumAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || !Enum.IsDefined(_enumType, value))
            {
                return new ValidationResult($"The {validationContext.DisplayName} field is not a valid {_enumType.Name} value.");
            }

            return ValidationResult.Success;
        }
    }
}
