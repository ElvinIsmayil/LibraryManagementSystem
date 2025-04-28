using Library_Management_System.Enums;
using Library_Management_System.ViewModels.AuthorContact;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.Author
{
    public class AuthorUpdateVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }  

        [StringLength(500)]
        public string Biography { get; set; }

        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

        public AuthorContactUpdateVM AuthorContactUpdateVM { get; set; }
    }
}
