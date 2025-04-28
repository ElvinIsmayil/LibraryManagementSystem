using Library_Management_System.Enums;
using Library_Management_System.ViewModels.AuthorContact;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.Author
{
    public class AuthorCreateVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        [StringLength(500)]
        public string? Biography { get; set; }

        public IFormFile? Image {  get; set; }

        [Required]
        public AuthorContactCreateVM AuthorContactCreateVM { get; set; }  

    }
}
  