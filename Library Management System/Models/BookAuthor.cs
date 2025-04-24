using Library_Management_System.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class BookAuthor : BaseEntity
    {
        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
