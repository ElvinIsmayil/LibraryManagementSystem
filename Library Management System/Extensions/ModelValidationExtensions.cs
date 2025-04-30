using Library_Management_System.ViewModels.Author;
using Library_Management_System.ViewModels.AuthorContact;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace Library_Management_System.Extensions
{
    public static class ModelValidationExtensions
    {
        public static void ValidateContactInfo(this AuthorContactCreateVM authorContactCreateVM, ModelStateDictionary modelState)
        {
            string phoneNumber = authorContactCreateVM.PhoneNumber;
            string email = authorContactCreateVM.Email;

            if (!string.IsNullOrEmpty(phoneNumber) || !string.IsNullOrEmpty(email))
            {
                Regex phoneNumberRegex = new Regex(@"^\+\d{1,4}\d{9}$");

                Regex emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

                if (!string.IsNullOrEmpty(phoneNumber) && !phoneNumberRegex.IsMatch(phoneNumber))
                {
                    modelState.AddModelError("PhoneNumber", "The phone number format is not correct! It should start with '+' followed by country code and number.");
                }

                if (!string.IsNullOrEmpty(email) && !emailRegex.IsMatch(email))
                {
                    modelState.AddModelError("Email", "The email format is not correct! Please ensure it follows a valid email format.");
                }
            }
        }
    }
}
