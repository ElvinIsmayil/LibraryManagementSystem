using Library_Management_System.Enums;
using Library_Management_System.ViewModels.AuthorContact;

namespace Library_Management_System.ViewModels.Author
{
    public class AuthorDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime? BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string Biography { get; set; }

        public string ImageUrl { get; set; }

        public AuthorContactDetailVM AuthorContactDetailVM { get; set; }

       
    }
}
