using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace User.Domain.ValidationAttributes
{
    public class PasswordContainsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var input = value as string;
            if (string.IsNullOrEmpty(input)) return ValidationResult.Success;

            if (!IsValidPassword(input))
            {
                return new ValidationResult("Your password must contain at least one big letter," +
                    " one small letter, one digit and one special character");
            }
            return ValidationResult.Success;

        }

        bool IsValidPassword(string password)
        {
            return
                Regex.IsMatch(password, @"[A-Z]") &&
                Regex.IsMatch(password, @"[a-z]") &&
                Regex.IsMatch(password, @"\d") &&
                Regex.IsMatch(password, @"[!@#$%^&*]");
        }
    }
}
