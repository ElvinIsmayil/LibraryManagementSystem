using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.BookCategory
{
    public class BookCategoryCreateVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
