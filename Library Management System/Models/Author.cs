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
        public DateTime? BirthDate { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public AuthorContact AuthorContact { get; set; }
    }
}
