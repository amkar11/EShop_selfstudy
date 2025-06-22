using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace User.Domain.ValidationAttributes
{
    public class NoAtSymbolAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var input = value as string;

            if (string.IsNullOrEmpty(input)) return ValidationResult.Success;

            if (input.Contains("@"))
            {
                return new ValidationResult("Your username must not contain \"@\" symbol");
            }

            return ValidationResult.Success;
        }
    }
}
