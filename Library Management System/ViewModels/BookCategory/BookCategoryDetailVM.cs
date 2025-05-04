using Library_Management_System.ViewModels.Book;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.BookCategory
{
    public class BookCategoryDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookVM> BookVMs { get; set; } = new List<BookVM>();
    }
}
