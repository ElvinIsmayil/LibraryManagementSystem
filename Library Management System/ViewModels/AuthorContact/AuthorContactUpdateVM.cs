using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.AuthorContact
{
    public class AuthorContactUpdateVM 
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; }
    }
}
