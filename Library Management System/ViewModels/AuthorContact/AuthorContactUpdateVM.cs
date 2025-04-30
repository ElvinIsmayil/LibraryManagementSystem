using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.AuthorContact
{
    public class AuthorContactUpdateVM 
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        
        public string PhoneNumber { get; set; }
    }
}
