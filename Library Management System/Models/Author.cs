using Library_Management_System.Enums;
using Library_Management_System.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Author : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [StringLength(500)]
        public string? Biography { get; set; }

        [DataType(DataType.Url)]
        public string ImageUrl { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

        [Required]
        public AuthorContact ContactDetails { get; set; }
    }
}
