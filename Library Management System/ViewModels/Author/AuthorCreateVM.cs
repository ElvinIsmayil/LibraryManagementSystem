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

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Url]
        public string ImageUrl { get; set; }
    }
}
