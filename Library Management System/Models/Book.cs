using Library_Management_System.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_System.Models
{
    public class Book : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; }


        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(1, 2000)]
        public int PageCount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [DataType(DataType.Url)]
        public string? ImageUrl { get; set; }

        public int BookCategoryId { get; set; }
        public BookCategory BookCategory { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }    

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
