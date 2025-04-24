using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.Book
{
    public class BookCreateVM
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Range(1, 2000)]
        public int PageCount { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [Url]
        public string? ImageUrl { get; set; } = "/img/book-cover-placeholder.png";

        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

    }
}
