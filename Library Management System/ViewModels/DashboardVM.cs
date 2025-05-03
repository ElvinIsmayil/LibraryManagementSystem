namespace Library_Management_System.ViewModels
{
    public class DashboardVM
    {
        public int TotalBooks { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalCategories { get; set; }
        public int TotalPublishers { get; set; }

        public int BooksAddedThisMonth { get; set; }
        public List<LatestBookItemVM> LatestBooks { get; set; }

        public List<string> CategoryNames { get; set; }
        public List<int> CategoryBookCounts { get; set; }


    }
}
