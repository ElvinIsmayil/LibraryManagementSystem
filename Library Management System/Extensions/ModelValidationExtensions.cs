using Library_Management_System.Models;
using Library_Management_System.ViewModels.Author;
using Library_Management_System.ViewModels.AuthorContact;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace Library_Management_System.Extensions
{
    public static class ModelValidationExtensions
    {
        public static void ValidateContactInfo(this AuthorContact authorContact, ModelStateDictionary modelState)
        {
            string phoneNumber = authorContact.PhoneNumber;
            string email = authorContact.Email;

            if (!string.IsNullOrEmpty(phoneNumber) || !string.IsNullOrEmpty(email))
            {
                if (!string.IsNullOrEmpty(phoneNumber) && !ValidatePhoneNumber(phoneNumber))
                {
                    modelState.AddModelError(nameof(AuthorContactUpdateVM.PhoneNumber), "The phone number format is not correct! It should start with '+' followed by country code and number.");
                }

                if (!string.IsNullOrEmpty(email) && !ValidateEmail(email))
                {
                    modelState.AddModelError(nameof(AuthorContactUpdateVM.Email), "The email format is not correct! Please ensure it follows a valid email format.");
                }
            }
        }
        private static bool ValidatePhoneNumber(string phoneNumber)
        {
            Regex phoneNumberRegex = new Regex(@"^\+\d{1,4}\d{9}$");
            return phoneNumberRegex.IsMatch(phoneNumber);
        }

        private static bool ValidateEmail(string email)
        {
            Regex emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }

        public static void ValidateBirthdate(this Author author, ModelStateDictionary modelState)
        {
            DateTime birthDate = author.BirthDate;

            if (birthDate > DateTime.Now)
            {
                modelState.AddModelError(nameof(author.BirthDate), "The birthdate cannot be in the future.");
            }

            var age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
            {
                age--;
            }
            if (age < 18)
            {
                modelState.AddModelError(nameof(author.BirthDate), "The author must be at least 18 years old.");
            }
        }
    }
}
