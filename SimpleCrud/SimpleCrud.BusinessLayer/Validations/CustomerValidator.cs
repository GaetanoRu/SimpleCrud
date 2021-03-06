using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SimpleCrud.Shared.Models.Requests;


namespace SimpleCrud.BusinessLayer.Validations
{
    public class CustomerValidator : AbstractValidator<SaveCustomerRequest>
    {
        private const string validatePhoneNumber = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";
        public CustomerValidator()
        {

            RuleFor(c => c.FirstName)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(50)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Must(IsValidFirstAndLastName).WithMessage("{PropertyName} should be all letters");


            RuleFor(c => c.LastName)
                .MaximumLength(50)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Must(IsValidFirstAndLastName).WithMessage("{PropertyName} should be all letters");

            RuleFor(c => c.StreetAddress)
                .MaximumLength(100);

            RuleFor(c => c.City)
                .MaximumLength(100);

            RuleFor(c => c.Country)
                .MaximumLength(50);

            RuleFor(c => c.PostalCode)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(5).WithMessage("The postcode must consist of 5 digits")
                .Must(CheckValidPostalCodeLenghtAndNumber).WithMessage("The Postal Code should only have numeric values");

            RuleFor(c => c.Email)
                .MaximumLength(50)
                .NotEmpty().WithMessage("{PropertyName} is required field")
                .EmailAddress().WithMessage("Email is not valid");


            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("{PropertyName} is required field")
                .Matches(validatePhoneNumber).WithMessage("{PropertyName} format is not valid");
        }

        private static bool CheckValidPostalCodeLenghtAndNumber(string postalCode)
        {
            bool isNumber = int.TryParse(postalCode, out int result);
            if (!string.IsNullOrWhiteSpace(postalCode))
            {
                return postalCode.Length == 5 && isNumber == true;

            }
            return isNumber || postalCode == null;
        }

        private static bool IsValidFirstAndLastName(string name)
        {

            return !string.IsNullOrEmpty(name) ? name.All(char.IsLetter) : false;
        }
    }
}
