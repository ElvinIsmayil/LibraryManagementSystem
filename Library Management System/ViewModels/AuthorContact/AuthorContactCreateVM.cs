using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.AuthorContact
{
    public class AuthorContactCreateVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        
        public string PhoneNumber { get; set; }
    }
}
