using Library_Management_System.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Publisher : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Adress {  get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
