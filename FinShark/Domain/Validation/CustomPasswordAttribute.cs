using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validation
{
    public class CustomPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(Object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(ErrorMessage ?? "Password is required.");
            }

            var password = value.ToString();

            if (password.Length < 8)
            {
                return new ValidationResult("Password must be at least 8 characters long.");
            }

            if (password.Length > 50)
            {
                return new ValidationResult("Passwords Cannot Be Longer Than 50 Characters.");
            }

            if (!HasSpecialCharacter(password))
            {
                return new ValidationResult("Password must contain at least one special character.");
            }

            return ValidationResult.Success;
        }

        private bool HasSpecialCharacter(string password)
        {
            return password.IndexOfAny(new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '.' }) >= 0;
        }
    }
}
