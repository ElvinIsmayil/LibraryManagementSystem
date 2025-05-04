using Library_Management_System.ViewModels.BookCategory;
using System.ComponentModel.DataAnnotations;
namespace Library_Management_System.ViewModels.Book
{
    public class BookVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public DateTime PublishedDate { get; set; }

        public string? ImageUrl { get; set; }

    }
}
