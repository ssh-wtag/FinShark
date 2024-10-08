using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validation
{
    public class CustomConfirmPasswordAttribute : ValidationAttribute
    {
        private readonly string _passwordPropertyName;

        public CustomConfirmPasswordAttribute(string passwordPropertyName)
        {
            _passwordPropertyName = passwordPropertyName;
        }


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(ErrorMessage ?? "Password Must Be Confirmed.");

            string? comparingPassword = value.ToString();

            var passwordProperty = validationContext.ObjectType.GetProperty(_passwordPropertyName);
            var passwordValue = passwordProperty.GetValue(validationContext.ObjectInstance)?.ToString();

            if (comparingPassword != passwordValue)
                return new ValidationResult(ErrorMessage ?? "Passwords Do Not Match.");


            return ValidationResult.Success;
        }
    }
}
