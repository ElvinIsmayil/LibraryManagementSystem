using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.Book
{
    public class BookUpdateVM
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(1, 2000)]
        public int PageCount { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; }

        public int BookCategoryId { get; set; }
        public int PublisherId { get; set; }
        public List<int> SelectedAuthorIds { get; set; } = new List<int>();

    }
}
