using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.BookCategory
{
    public class BookCategoryUpdateVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
