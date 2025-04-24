using Library_Management_System.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_System.Models
{
    public class AuthorContact : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public int AuthorId { get; set; }
        public Author Author {  get; set; }

    }
}
