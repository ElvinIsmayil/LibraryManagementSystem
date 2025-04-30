using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.Book
{
    public class BookDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public DateTime PublishedDate { get; set; }

        public string? ImageUrl { get; set; }

        public string BookCategoryName { get; set; }  
        public string PublisherName { get; set; } 
        public List<string> AuthorNames { get; set; }


    }
}
