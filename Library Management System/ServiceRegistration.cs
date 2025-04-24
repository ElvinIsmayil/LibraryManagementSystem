using Microsoft.EntityFrameworkCore;
using Library_Management_System.DAL;

namespace Library_Management_System
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration) // => Extension
        {
            services.AddDbContext<LibraryManagementSystemDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
