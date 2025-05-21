using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// A custom validation attribute used to validate CPF addresses.
    /// </summary>
    public class ValidCpfAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value is a valid CPF address.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><c>true</c> if the value is a valid CPF address; otherwise, <c>false</c>.</returns>
        public override bool IsValid(object? value)
        {
            var cpf = value as string;
            if (cpf == null) return false;

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || !Regex.IsMatch(cpf, @"^\d{11}$"))
                return false;

            // Validate the CPF algorithmically
            var cpfArray = cpf.ToCharArray();

            int[] multipliersFirstDigit = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multipliersSecondDigit = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string firstNineDigits = new string(cpfArray, 0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(firstNineDigits[i].ToString()) * multipliersFirstDigit[i];
            }

            int firstVerificationDigit = 11 - (sum % 11);
            firstVerificationDigit = firstVerificationDigit >= 10 ? 0 : firstVerificationDigit;

            sum = 0;
            string firstTenDigits = new string(cpfArray, 0, 10);

            for (int i = 0; i < 10; i++)
            {
                sum += int.Parse(firstTenDigits[i].ToString()) * multipliersSecondDigit[i];
            }

            int secondVerificationDigit = 11 - (sum % 11);
            secondVerificationDigit = secondVerificationDigit >= 10 ? 0 : secondVerificationDigit;

            return cpf.EndsWith(firstVerificationDigit.ToString() + secondVerificationDigit.ToString());
        }
    }
}
