using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.BookCategory
{
    public class BookCategoryCreateVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(300)]
        public string? Description { get; set; }
    }
}
