using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.Publisher
{
    public class PublisherCreateVM
    {
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Adress { get; set; }

        [Required]
        public double Rating { get; set; }
        
    }
}
