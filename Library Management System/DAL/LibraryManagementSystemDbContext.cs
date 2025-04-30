using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.DAL
{
    public class LibraryManagementSystemDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorContact> AuthorContacts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public LibraryManagementSystemDbContext(DbContextOptions<LibraryManagementSystemDbContext> options) : base(options) { }
        
        
    }
}
