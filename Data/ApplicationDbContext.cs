using Microsoft.EntityFrameworkCore;
using Order_Management.Models;
namespace Order_Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
        
        public DbSet<User> User { get; set; } // DbSet for User entity
        // Add DbSet properties for other entities as needed
    }
}